(()=>{var e={548:()=>{document.addEventListener("DOMContentLoaded",(function(){document.addEventListener("click",(function(e){"modal-close"==e.target.id&&(e.preventDefault(),document.querySelector(".modal").classList.remove("is-active"))}),!1)}))}},t={};function r(n){var i=t[n];if(void 0!==i)return i.exports;var a=t[n]={exports:{}};return e[n](a,a.exports,r),a.exports}r.n=e=>{var t=e&&e.__esModule?()=>e.default:()=>e;return r.d(t,{a:t}),t},r.d=(e,t)=>{for(var n in t)r.o(t,n)&&!r.o(e,n)&&Object.defineProperty(e,n,{enumerable:!0,get:t[n]})},r.o=(e,t)=>Object.prototype.hasOwnProperty.call(e,t),(()=>{"use strict";r(548),(new Object).PrintPricingsPage=new function(){const{createApp:e}=Vue;this.el=document.getElementById("main-app"),e({data:()=>({form:{brandId:0,currency:""},validation:{supplier:!0,currency:!0},formErrorMessage:""}),mounted(){},methods:{validate(){this.validation.supplier=!0,this.validation.currency=!0;let e=!1;return 0==this.form.brandId&&(this.validation.supplier=!1,e=!0),""==this.form.currency&&(this.validation.currency=!1,e=!0),!e},print(){this.validate()&&(location=`/print-pricing?brandid=${this.form.brandId}&currency=${this.form.currency}`)}}}).mount("#main-app")}})()})();