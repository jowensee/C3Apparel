(()=>{var i={548:()=>{document.addEventListener("DOMContentLoaded",(function(){document.addEventListener("click",(function(i){"modal-close"==i.target.id&&(i.preventDefault(),document.querySelector(".modal").classList.remove("is-active"))}),!1)}))}},e={};function r(t){var c=e[t];if(void 0!==c)return c.exports;var o=e[t]={exports:{}};return i[t](o,o.exports,r),o.exports}r.n=i=>{var e=i&&i.__esModule?()=>i.default:()=>i;return r.d(e,{a:e}),e},r.d=(i,e)=>{for(var t in e)r.o(e,t)&&!r.o(i,t)&&Object.defineProperty(i,t,{enumerable:!0,get:e[t]})},r.o=(i,e)=>Object.prototype.hasOwnProperty.call(i,e),(()=>{"use strict";r(548),(new Object).PricingEdit=new function(){const{createApp:i}=Vue;let e=this;this.el=document.getElementById("editMain");const r=e.el.getAttribute("data-endpoint-get"),t=e.el.getAttribute("data-endpoint-save"),c=e.el.getAttribute("data-method");i({data:()=>({pricing:{id:Number(e.el.getAttribute("data-id")),status:"",brandId:0,collection:"",c3Style:"",supplierStyle:"",description:"",coo:"",productGroup:"",productSubCategory:"",sizes:"",allSizes:"",productColours:"",colourDescriptions:"",buyPrice:0,skuWeight:0,c3OverrideWeight:0},formErrorMessage:"",showSuccess:!1,errors:[]}),mounted(){this.pricing.id>0&&this.populateForm(this.pricing.id)},methods:{populateForm(i){let e={id:i},t=this.pricing;fetch(r,{method:c,headers:{"Content-Type":"application/json"},body:"GET"==c?null:JSON.stringify(e),data:"GET"!=c?null:JSON.stringify(e)}).then((i=>i.json())).then((function(i){t.status=i.productPricing.status,t.brandId=i.productPricing.brandId,t.collection=i.productPricing.collection,t.c3Style=i.productPricing.c3Style,t.supplierStyle=i.productPricing.supplierStyle,t.description=i.productPricing.description,t.coo=i.productPricing.coo,t.productGroup=i.productPricing.productGroup,t.productSubCategory=i.productPricing.productSubCategory,t.sizes=i.productPricing.sizes,t.allSizes=i.productPricing.allSizes,t.productColours=i.productPricing.colour,t.colourDescriptions=i.productPricing.colourDescription,t.buyPrice=i.productPricing.c3BuyPrice,t.skuWeight=i.productPricing.skuWeight,t.c3OverrideWeight=i.productPricing.c3OverrideWeight}))},validiteForm(){this.errors=[];let i=!0;return 0==this.pricing.brandId&&(this.errors.push("<b>Brand:</b> Please select one"),i=!1),""==this.pricing.c3Style.trim()&&(this.errors.push("<b>C-3 Style:</b> Please enter a value"),i=!1),""==this.pricing.description.trim()&&(this.errors.push("<b>Description:</b> Please enter a value"),i=!1),""==this.pricing.productGroup.trim()&&(this.errors.push("<b>Product Group:</b> Please enter a value"),i=!1),""==this.pricing.sizes.trim()&&(this.errors.push("<b>Sizes:</b> Please enter a value"),i=!1),""==this.pricing.allSizes.trim()&&(this.errors.push("<b>All Sizes:</b> Please enter a value"),i=!1),""==this.pricing.colourDescriptions.trim()&&(this.errors.push("<b>Colour Descriptions:</b> Please enter a value"),i=!1),isNaN(this.pricing.buyPrice)&&(this.errors.push("<b>Buy Price:</b> Please enter a decimal value"),i=!1),isNaN(this.pricing.skuWeight)&&(this.errors.push("<b>SKU Weight:</b> Please enter a decimal value"),i=!1),isNaN(this.pricing.c3OverrideWeight)&&(this.errors.push("<b>C3 Override Weight:</b> Please enter a decimal value"),i=!1),i},savePricing(){if(this.showSuccess=!1,!this.validiteForm())return;let i=this,e={id:i.pricing.id,status:i.pricing.status,brandId:i.pricing.brandId,collection:i.pricing.collection,c3Style:i.pricing.c3Style,supplierStyle:i.pricing.supplierStyle,description:i.pricing.description,coo:i.pricing.coo,productGroup:i.pricing.productGroup,productSubCategory:i.pricing.productSubCategory,sizes:i.pricing.sizes,allSizes:i.pricing.allSizes,colour:i.pricing.productColours,colourDescription:i.pricing.colourDescriptions,c3BuyPrice:i.pricing.buyPrice,skuWeight:i.pricing.skuWeight,c3OverrideWeight:i.pricing.c3OverrideWeight};fetch(t,{method:c,headers:{"Content-Type":"application/json"},body:"GET"==c?null:JSON.stringify(e),data:"GET"!=c?null:JSON.stringify(e)}).then((i=>i.json())).then((function(e){e.success?i.showSuccess=!0:i.formErrorMessage=e.message}))}}}).mount("#editMain")}})()})();