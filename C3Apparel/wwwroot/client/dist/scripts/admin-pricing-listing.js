(()=>{var t={548:()=>{document.addEventListener("DOMContentLoaded",(function(){document.addEventListener("click",(function(t){"modal-close"==t.target.id&&(t.preventDefault(),document.querySelector(".modal").classList.remove("is-active"))}),!1)}))}},e={};function i(r){var l=e[r];if(void 0!==l)return l.exports;var o=e[r]={exports:{}};return t[r](o,o.exports,i),o.exports}i.n=t=>{var e=t&&t.__esModule?()=>t.default:()=>t;return i.d(e,{a:e}),e},i.d=(t,e)=>{for(var r in e)i.o(e,r)&&!i.o(t,r)&&Object.defineProperty(t,r,{enumerable:!0,get:e[r]})},i.o=(t,e)=>Object.prototype.hasOwnProperty.call(t,e),(()=>{"use strict";i(548);var t=new Object;function e(){const{createApp:t}=Vue;let e=this;this.el=document.getElementById("main-app"),this.itemsPerPage=Number(this.el.getAttribute("data-item-per-page"));const i=e.el.getAttribute("data-endpoint"),r=e.el.getAttribute("data-delete-endpoint"),l=e.el.getAttribute("data-method"),o=e.el.getAttribute("data-initial-filter");t({data:()=>({showDeletePopup:!1,deleteId:0,grid:{pagination:{currentPage:1,totalPage:0,rowPerPage:20},rows:[]},filter:{brandId:0,c3Style:"",collection:"",supplierStyle:"",description:"",coo:"",productGroup:"",sizes:"",colour:""},searchClicked:!1,filterValidationError:""}),computed:{showError:function(){return 0==this.grid.rows.length&&this.searchClicked}},mounted(){console.log("initialFilterObject",o);var t=JSON.parse(o);null!=t&&(this.filter.brandId=t.filters.filterSupplier,this.filter.c3Style=t.filters.filterC3Style,this.filter.collection=t.filters.filterCollection,this.filter.supplierStyle=t.filters.filterSupplierStyle,this.filter.description=t.filters.filterDescription,this.filter.coo=t.filters.filterCOO,this.filter.productGroup=t.filters.filterProductGroup,this.filter.sizes=t.filters.filterSizes,this.filter.colour=t.filters.filterColour),this.filter.brandId>0&&this.populateGrid(null==t||0==t.pageNumber?1:t.pageNumber)},methods:{populateGrid(t){let r={filters:{filterSupplier:this.filter.brandId,filterC3Style:this.filter.c3Style,filterCollection:this.filter.collection,filterSupplierStyle:this.filter.supplierStyle,filterDescription:this.filter.description,FilterCOO:this.filter.coo,filterProductGroup:this.filter.productGroup,filterSizes:this.filter.sizes,filterColour:this.filter.colour},pageNumber:t,itemsPerPage:e.itemsPerPage},o=this.grid;fetch(i,{method:l,headers:{"Content-Type":"application/json"},body:"GET"==l?null:JSON.stringify(r),data:"GET"!=l?null:JSON.stringify(r)}).then((t=>t.json())).then((function(t){o.pagination.totalPage=t.totalPage,void 0!==t.pricings&&(o.rows=[],t.pricings.forEach((t=>{o.rows.push(t)})))}))},filterResults(){""!=this.filter.brandId?(this.searchClicked=!0,this.filterValidationError="",this.populateGrid(1)):this.filterValidationError="Please select a brand."},gotoPage(t){this.grid.pagination.currentPage=Number(t.srcElement.value),this.populateGrid(this.grid.pagination.currentPage)},gotoPreviousPage(t){t.preventDefault(),this.grid.pagination.currentPage>1&&(this.grid.pagination.currentPage--,this.populateGrid(this.grid.pagination.currentPage))},gotoNextPage(t){t.preventDefault(),this.grid.pagination.currentPage<this.grid.pagination.totalPage&&(this.grid.pagination.currentPage++,this.populateGrid(this.grid.pagination.currentPage))},loadDeleteConfirmation(t){this.showDeletePopup=!0,this.deleteId=t},deleteProductPrice(){let t={id:this.deleteId};fetch(r,{method:l,headers:{"Content-Type":"application/json"},body:"GET"==l?null:JSON.stringify(t),data:"GET"!=l?null:JSON.stringify(t)}).then((t=>t.json())).then((function(t){t.success?location.reload():console.log(t.message)}))}}}).mount("#main-app")}document.addEventListener("DOMContentLoaded",(function(){t.PricingListing=new e}))})()})();