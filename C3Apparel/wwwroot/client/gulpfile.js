const gulp = require('gulp');
const fs = require('fs');
const del = require('del');
const glob = require('glob');
const path = require('path');
const sass = require('gulp-sass')(require('sass'));
const concat = require('gulp-concat');
const uglify = require('gulp-uglify');
const sourcemaps = require('gulp-sourcemaps');
const cond = require('gulp-cond'); 
const browserSync = require('browser-sync').create();
const babel = require("gulp-babel");
const plumber = require("gulp-plumber");
const q = require('q'); // Promise library

// HTML
// NOTE: 'gulp-file-include' and 'gulp-html-extend' to similar things.
const fileInclude = require('gulp-file-include'); // used to import our 'widgets' mark-up into 'pages'.
const htmlExtender = require('gulp-html-extend'); // used to import 'scripts' and 'widget/pages' mark-up into our 'master page'.
const htmlBeautify = require('gulp-html-beautify');
const devIp = require('dev-ip'); // Find a suitable IP host to view local websites on

// Creating starter files
const prompt = require('gulp-prompt');
const toSlugCase = require('to-slug-case');
const toPascalCase = require('to-pascal-case');

const webpack = require('webpack');
const webpackStream = require('webpack-stream');

// Environment functions
let baseDir = '.tmp'

function setEnvDev(cb) {
    process.env.NODE_ENV = 'development';
    baseDir = '.tmp'
    cb()
}

function setEnvProd(cb) {
    process.env.NODE_ENV = 'production';
    baseDir = 'dist'
    cb()
}

function browser() {
    return browserSync.init({
        watch: true, // `true` reloads browser on changes
        server: ['.tmp', 'src'],
        port: 3030,
        host: devIp()[0],
        reloadDelay: 500
    });
}


gulp.task('sass', function(){
    return gulp.src('src/styles')
        .pipe(sourcemaps.init())
        .pipe(sass({ outputStyle: 'compressed' })) // Converts Sass to CSS with gulp-sass
        .pipe(sourcemaps.write(`./maps`))
        .pipe(gulp.dest(baseDir + '/styles'))
});

gulp.task('watch', function(){
    gulp.watch('src/styles/*.scss', gulp.series("sass"));
   // gulp.watch('js/src/*.js', gulp.series("scripts"));

});

gulp.task('browser-sync', function() {
    browserSync.init({
        server: {
            baseDir: "./"
        },
        port: 3030,
        host: devIp()[0]
    });
});



gulp.task('default', gulp.series('watch'));


function cleanDist(cb) {
    del(['dist/*']);
    cb()
}


function cleanTemp(cb) {
    del(['.tmp/*']);
    cb()
}

// CSS functions
async function buildCssFileWithDartSass(filePath) {

    const defer = q.defer();

    gulp.src(filePath)
        .pipe(cond(process.env.NODE_ENV === 'development', sourcemaps.init()))
        .pipe(sass().on('error', sass.logError))
        //.pipe(postcss([autoprefixer()]))
        .pipe(cond(process.env.NODE_ENV === 'development', sourcemaps.write('.')))
        .pipe(gulp.dest(baseDir + '/styles'))
        .on('end', function() {
            defer.resolve()
        })
        .on('error', function() {
            defer.reject()
        });

    return defer.promise;
}

async function buildCssAsync(cb) {
    const files = [
        'src/styles/global.scss', // Build the 'global.css' as separate file.
    ];

    glob.sync('src/pages/*/*.scss').forEach(function(filePath) {
        files.push(filePath);
    });

    for (let i = 0; i < files.length; i++) {
        console.log('Building : ' + files[i])
        await buildCssFileWithDartSass(files[i]).then(function() {
            console.log('Finished: ' + files[i]);
        })
    }

    cb();
}


async function buildJsFile(filePath) {
    const defer = q.defer();

    gulp.src(filePath)
        .pipe(webpackStream({
            mode: process.env.NODE_ENV,
            output: {
                filename: 'scripts/' + path.basename(filePath)
            },
            module: {
                rules: [
                    {
                        test: /\.js$/,
                        exclude: /node_modules/,
                        use: {
                            loader: 'babel-loader'
                        }
                    }
                ]
            }
        }, webpack))
        .pipe(gulp.dest(baseDir))
        .on('end', function() {
            defer.resolve()
        })
        .on('error', function() {
            defer.reject()
        })

    return defer.promise
}
async function buildJsAsync(cb) {
    const files = [];

    glob.sync('src/pages/*/*.js', {"ignore": ['__GULP_IGNORE__']}).forEach(function(filePath) {
        files.push(filePath)
    });

    console.log(files)

    for (let i = 0; i < files.length; i++) {
        // console.log('Building: ' + files[i])
        await buildJsFile(files[i]).then(function() {
            console.log('Finished: ' + files[i]);
        })
    }

    cb();
}

/*function buildJS(){
    return gulp.src('src/scripts/*.js')
        //.pipe(sourcemaps.init())
        .pipe(plumber())
        .pipe(
            babel({
            presets: [
                [
                "@babel/env",
                {
                    modules: false,
                },
                ],
            ],
            })
        )
        //.pipe(concat('src/scripts/scripts.js'))
        .pipe(uglify())
        .pipe(gulp.dest(baseDir + '/js'))
};*/

async function buildHtmlFile(filePath) {
    const defer = q.defer();

    gulp.src(filePath)
        .pipe(htmlExtender({
            root: '/' // include file relative to the dir in which gulp is running
        }))
        .pipe(fileInclude({
            basepath: '@root' // include file relative to the dir in which gulp is running
        }))
        .pipe(htmlBeautify({
            indent_char: ' ',
            indent_size: 2
        }))
        .pipe(gulp.dest(baseDir))
        .on('end', function() {
            defer.resolve()
        })
        .on('error', function() {
            defer.reject()
        })

    return defer.promise
}

async function buildHtmlAsync(cb) {
    const files = [
        'src/pages/index.html',
    ];

    glob.sync('src/pages/*/*.html').forEach(function(filePath) {
        files.push(filePath);
    });

    for (let i = 0; i < files.length; i++) {
        console.log('Building: ' + files[i])
        await buildHtmlFile(files[i]).then(function() {
            console.log('Finished: ' + files[i]);
        })
    }

    cb();
}

function copyImages(cb) {
    gulp.src('src/images/**/*')
        .pipe(gulp.dest(baseDir + '/images'));
    cb()
}

function copyBulmaCSS(cb) {
    gulp.src('src/styles/bulma/css/bulma.min.css')
        .pipe(gulp.dest(baseDir + '/styles'));
    cb()
}


// Server functions
function browser() {
    return browserSync.init({
        watch: true, // `true` reloads browser on changes
        server: ['.tmp', 'src'],
        port: 3030,
        host: devIp()[0],
        reloadDelay: 500
    });
}


// Watch functions
function watch(cb) {
    gulp.watch('src/**/*.js', gulp.series(buildJsAsync))
    gulp.watch('src/**/*.scss', gulp.series(buildCssAsync))
    gulp.watch('src/**/*.html', gulp.series(buildHtmlAsync))
    cb()
}


// Create new 'widget'
gulp.task('new-widget', function(cb) {
    gulp.src('./')
        .pipe(prompt.prompt({
            type: 'input',
            name: 'filename',
            message: 'Please specify a file name (in kebab-case):'
        }, function(res) {
            var cls = toPascalCase(res.filename);
            var slug = toSlugCase(res.filename);
            console.log('Creating widget ' + res.filename);
            console.log('@@include(\'src/widgets/' + slug + '/' + slug + '.html\')')
            console.log('import \'../../widgets/' + slug + '/' + slug + '\'')
            console.log('@import "../../widgets/' + slug + '/' + slug + '.scss";')

            if (res.filename) {
                var widgetPath = 'src/widgets/' + slug;
                if (!fs.existsSync(widgetPath + '/' + slug + '/' + slug + '.html')) {
                    fs.mkdirSync(widgetPath);

                    fs.writeFile(widgetPath + '/' + slug + '.html', '<!-- START widget ' + slug + ' -->\n<div class="' + slug + '">\n' + slug + '\n</div>\n<!-- END widget ' + slug + ' -->', function() {
                        fs.writeFile(widgetPath + '/' + slug + '.scss', '@import "../../styles/variables.scss";\n@import "../../styles/mixins.scss";\n\n.' + slug + ' {\n\n}', function() {
           
                        });
                    });
                } else {
                    console.log(res.filename + ' already exists');
                }
            } else {
                console.log('You must specify a filename');
            }
        }));
    cb();
});

// Create new 'page'
gulp.task('new-page', function(cb) {
    gulp.src('./')
        .pipe(prompt.prompt({
            type: 'input',
            name: 'filename',
            message: 'Please specify a file name (in kebab-case):'
        }, function(res) {
            var slug = toSlugCase(res.filename);
            console.log('Creating page ' + res.filename);

            if (res.filename) {
                var widgetPath = 'src/pages/' + slug;
                if (!fs.existsSync(widgetPath + '/' + slug + '/' + slug + '.html')) {
                    fs.mkdirSync(widgetPath);

                    fs.writeFile(widgetPath + '/' + slug + '.html', '<!-- @@master = /src/masterpages/default.html-->\n' +
                        '\n' +
                        '<!-- @@block = head-->\n' +
                        '<link rel="stylesheet" href="styles/' + slug + '.css">\n' +
                        '<!-- @@close-->\n' +
                        '\n' +
                        '<!-- @@block = body-->\n' +
                        '<!--@@include(\'src/widgets/xxx/xxx.html\')-->\n' +
                        '<!-- @@close-->\n' +
                        '\n' +
                        '<!-- @@block = scripts-->\n' +
                        '<script src="scripts/' + slug + '.js"></script>\n' +
                        '<!-- @@close-->', function() {
                        fs.writeFile(widgetPath + '/' + slug + '.scss', '//@import "../../widgets/xxx/xxx";\n', function() {
                            fs.writeFile(widgetPath + '/' + slug + '.js', 'import "../../scripts/global"\n\n//import "../../widgets/xxx/xxx"\n', function() {

                            });
                        });
                    });
                } else {
                    console.log(res.filename + ' already exists');
                }
            } else {
                console.log('You must specify a filename');
            }
        }));
    cb();
});



// Serve for development
gulp.task('serve', gulp.series(setEnvDev, cleanTemp, buildCssAsync, buildJsAsync,copyImages,buildHtmlAsync, watch, copyBulmaCSS, browser));

gulp.task('build', gulp.series(setEnvProd, cleanDist, buildCssAsync, buildJsAsync,copyImages,copyBulmaCSS, buildHtmlAsync));