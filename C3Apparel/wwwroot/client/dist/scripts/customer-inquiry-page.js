(()=>{var t={548:()=>{document.addEventListener("DOMContentLoaded",(function(){document.addEventListener("click",(function(t){"modal-close"==t.target.id&&(t.preventDefault(),document.querySelector(".modal").classList.remove("is-active"))}),!1)}))}},e={};function i(r){var o=e[r];if(void 0!==o)return o.exports;var n=e[r]={exports:{}};return t[r](n,n.exports,i),n.exports}i.n=t=>{var e=t&&t.__esModule?()=>t.default:()=>t;return i.d(e,{a:e}),e},i.d=(t,e)=>{for(var r in e)i.o(e,r)&&!i.o(t,r)&&Object.defineProperty(t,r,{enumerable:!0,get:e[r]})},i.o=(t,e)=>Object.prototype.hasOwnProperty.call(t,e),(()=>{"use strict";i(548);var t=new Object;function e(){const{createApp:t}=Vue;let e=this;this.el=document.getElementById("main-app"),this.currency=this.el.getAttribute("data-currency"),this.itemsPerPage=Number(this.el.getAttribute("data-item-per-page"));const i=e.el.getAttribute("data-endpoint"),r=e.el.getAttribute("data-endpoint-download"),o=e.el.getAttribute("data-method");t({data:()=>({grid:{pagination:{currentPage:1,totalPage:0,rowPerPage:20},rows:[]},filter:{brandId:0,c3Style:"",collection:"",description:"",colourDesc:"",productGroup:"",sizes:"",colour:""}}),mounted(){},methods:{populateGrid(t){console.log("populate");let r={filters:{brandId:this.filter.brandId,c3Style:this.filter.c3Style,collection:this.filter.collection,description:this.filter.description,productGroup:this.filter.productGroup,sizes:this.filter.sizes,colours:this.filter.colour,colourDescriptions:this.filter.colourDesc},currency:e.currency,pageNumber:t,itemsPerPage:e.itemsPerPage},n=this.grid;fetch(i,{method:o,headers:{"Content-Type":"application/json"},body:"GET"==o?null:JSON.stringify(r),data:"GET"!=o?null:JSON.stringify(r)}).then((t=>t.json())).then((function(t){n.pagination.totalPage=t.totalPage,void 0!==t.pricings&&(n.rows=[],t.pricings.forEach((t=>{n.rows.push(t)})))}))},gotoPage(t){this.grid.pagination.currentPage=Number(t.srcElement.value),this.populateGrid(this.grid.pagination.currentPage)},gotoPreviousPage(t){t.preventDefault(),this.grid.pagination.currentPage>1&&(this.grid.pagination.currentPage--,this.populateGrid(this.grid.pagination.currentPage))},gotoNextPage(t){t.preventDefault(),this.grid.pagination.currentPage<this.grid.pagination.totalPage&&(this.grid.pagination.currentPage++,this.populateGrid(this.grid.pagination.currentPage))},filterResults(){this.populateGrid(1)},download(){this.errorMessage="";let t={filters:{brandId:this.filter.brandId,c3Style:this.filter.c3Style,collection:this.filter.collection,description:this.filter.description,productGroup:this.filter.productGroup,sizes:this.filter.sizes,colours:this.filter.colour,colourDescriptions:this.filter.colourDesc},currency:e.currency};fetch(r,{method:"POST",headers:{"Content-Type":"application/json"},body:JSON.stringify(t),data:null}).then((t=>t.blob())).then((t=>{var e=window.URL.createObjectURL(t),i=document.createElement("a");i.href=e,i.download="pricelistinquiry.csv",document.body.appendChild(i),i.click(),i.remove()}))}}}).mount("#main-app")}document.addEventListener("DOMContentLoaded",(function(){t.PriceListing=new e}))})()})();