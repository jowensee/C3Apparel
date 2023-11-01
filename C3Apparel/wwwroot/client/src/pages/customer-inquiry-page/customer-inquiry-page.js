import "../../scripts/global"
//import Multiselect from '@vueform/multiselect'


var Page = new Object()

function PriceListing(){
    //const { createApp } = Vue
    let thisObject = this;
    this.el = document.getElementById("main-app");
    this.currency =  this.el.getAttribute("data-currency") ;
    this.itemsPerPage = Number(this.el.getAttribute("data-item-per-page"))
    const endpoint = thisObject.el.getAttribute("data-endpoint")    
    const downloadEndpoint = thisObject.el.getAttribute("data-endpoint-download")
    const method = thisObject.el.getAttribute("data-method")

    const app = Vue.createApp({
        //components: {
        //    Multiselect,
        //  },
        data() {
            return {
                
                grid: {
                    pagination:{
                        currentPage:1,
                        totalPage:0,
                        rowPerPage:20
                    },
                    rows:[]
                },
                filter:{
                    brands:[],
                    c3Style:'',
                    collection:'',
                    description:'',
                    colourDesc:'',
                    productGroup:'',
                    sizes:'',
                    colour:''
                },
                searchClicked:false,
                brandOptions:[],
                hasError:false,
                errorMessage:''
            }
        },
        mounted(){

           this.brandOptions = JSON.parse(document.getElementById('hiddenOptions').value)
        },
        computed:{
            showError: function(){
                return this.grid.rows.length == 0 && this.searchClicked
            }

        },
        methods:{
            populateGrid(pageNumber){

                var brandIds = []
                this.filter.brands.forEach(element => {
                    brandIds.push(element)
                });

                let data = {


                    filters:{
                        brands: brandIds,
                        c3Style:this.filter.c3Style,
                        collection:this.filter.collection,
                        description:this.filter.description,
                        productGroup:this.filter.productGroup,
                        sizes:this.filter.sizes,
                        colours:this.filter.colour,
                        colourDescriptions:this.filter.colourDesc
                     },
                    currency:thisObject.currency,
                    pageNumber:pageNumber,
                    itemsPerPage:thisObject.itemsPerPage

                }

                
                let grid = this.grid;
                fetch(endpoint, {
                    method: method,
                    headers:{
                        'Content-Type': 'application/json'  
                    },
                    body: method == "GET" ? null : JSON.stringify(data),
                    data: method != "GET" ? null : JSON.stringify(data),
                }).then(res=>res.json())
                    .then(function (response) {
                        grid.pagination.totalPage = response.totalPage;
                        if (response.pricings !== undefined)
                        {
                            grid.rows = []
                            response.pricings.forEach(element => {
                                grid.rows.push(element)
                            });
                        }
                    });

            },
            gotoPage(event){
                this.grid.pagination.currentPage = Number(event.srcElement.value);
                this.populateGrid(this.grid.pagination.currentPage)
            },
            gotoPreviousPage(event){
                event.preventDefault();
                if (this.grid.pagination.currentPage > 1){
                    this.grid.pagination.currentPage--;
                    this.populateGrid(this.grid.pagination.currentPage)
                }
                
            },
            gotoNextPage(event){
                event.preventDefault();
                if (this.grid.pagination.currentPage < this.grid.pagination.totalPage){
                    this.grid.pagination.currentPage++;
                    this.populateGrid(this.grid.pagination.currentPage)
                }
            },
            filterResults(){
                
                this.searchClicked = true
                this.populateGrid(1);
            
            },
            download(){
                this.errorMessage = ''
                this.hasError = false
                var brandIds = []
                this.filter.brands.forEach(element => {
                    brandIds.push(element)
                });

                let data = {
                    filters:{
                        brands: brandIds,
                        c3Style:this.filter.c3Style,
                        collection:this.filter.collection,
                        description:this.filter.description,
                        productGroup:this.filter.productGroup,
                        sizes:this.filter.sizes,
                        colours:this.filter.colour,
                        colourDescriptions:this.filter.colourDesc
                     },
                    currency:thisObject.currency

                }

                let _this = this;
                fetch(downloadEndpoint, {
                    method: "POST",
                    headers:{
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(data),
                    data: null
                })/*.then( res => res.blob() )
                    .then( blob => {
                        console.log('blob', blob)
                        var url = window.URL.createObjectURL(blob);
                        var a = document.createElement('a');
                        a.href = url;
                        a.download = `C-3pricelistinquiry.csv`;
                        document.body.appendChild(a); // we need to append the element to the dom -> otherwise it will not work in firefox
                        a.click();
                        a.remove();  
                    })*/
                   .then(function (response) {

                        const contentType = response.headers.get("content-type");
                        if (contentType && contentType.indexOf("application/json") !== -1) {
                            return response.json().then(data => {
                                _this.hasError = true
                                _this.errorMessage = data.errorMessage
                            });
                        } else {
                            return response.blob().then(function(blob) {
    
                                var url = window.URL.createObjectURL(blob);
                                var a = document.createElement('a');
                                a.href = url;
                                a.download = `C-3pricelistinquiry.csv`;
                                document.body.appendChild(a); // we need to append the element to the dom -> otherwise it will not work in firefox
                                a.click();
                                a.remove();  
                            });
                        }

                        
                    });

            },
        }
    })
    app.component('Multiselect', VueformMultiselect)
    app.mount('#main-app')
}

document.addEventListener("DOMContentLoaded", function(){


    Page.PriceListing = new PriceListing();
})
