(()=>{var e={548:()=>{document.addEventListener("DOMContentLoaded",(function(){document.addEventListener("click",(function(e){"modal-close"==e.target.id&&(e.preventDefault(),document.querySelector(".modal").classList.remove("is-active"))}),!1)}))}},t={};function n(i){var o=t[i];if(void 0!==o)return o.exports;var r=t[i]={exports:{}};return e[i](r,r.exports,n),r.exports}n.n=e=>{var t=e&&e.__esModule?()=>e.default:()=>e;return n.d(t,{a:t}),t},n.d=(e,t)=>{for(var i in t)n.o(t,i)&&!n.o(e,i)&&Object.defineProperty(e,i,{enumerable:!0,get:t[i]})},n.o=(e,t)=>Object.prototype.hasOwnProperty.call(e,t),(()=>{"use strict";n(548),(new Object).UploadPricingsPage=new function(){const{createApp:e}=Vue;this.el=document.getElementById("main-app");const t=this.el.getAttribute("data-endpoint-upload"),n=this.el.getAttribute("data-method");document.getElementById("file"),e({data:()=>({form:{brandId:0,file:null,deleteAll:!1},validation:{supplier:!0,file:!0},formErrorMessage:""}),mounted(){},methods:{validate(){this.validation.supplier=!0,this.validation.file=!0;let e=!1;return 0==this.form.brandId&&(this.validation.supplier=!1,e=!0),0==this.$refs.file.files.length&&(this.validation.file=!1,e=!0),!e},upload(){if(!this.validate())return;let e=this,i={brandId:e.form.brandId,deleteAll:e.form.deleteAll};const o=new FormData;for(const e in i)i.hasOwnProperty(e)&&o.append(e,i[e]);this.$refs.file.files.length>0?o.append("file",this.$refs.file.files[0]):o.append("file",null),console.log(i),fetch(t,{method:n,body:o}).then((e=>e.json())).then((function(t){e.formErrorMessage=t.message}))}}}).mount("#main-app")}})()})();