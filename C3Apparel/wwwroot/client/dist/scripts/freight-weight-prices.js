(()=>{var e={548:()=>{document.addEventListener("DOMContentLoaded",(function(){document.addEventListener("click",(function(e){"modal-close"==e.target.id&&(e.preventDefault(),document.querySelector(".modal").classList.remove("is-active"))}),!1)}))}},t={};function r(i){var s=t[i];if(void 0!==s)return s.exports;var n=t[i]={exports:{}};return e[i](n,n.exports,r),n.exports}r.n=e=>{var t=e&&e.__esModule?()=>e.default:()=>e;return r.d(t,{a:t}),t},r.d=(e,t)=>{for(var i in t)r.o(t,i)&&!r.o(e,i)&&Object.defineProperty(e,i,{enumerable:!0,get:t[i]})},r.o=(e,t)=>Object.prototype.hasOwnProperty.call(e,t),(()=>{"use strict";r(548),(new Object).SetSettings=new function(){const{createApp:e}=Vue;let t=this;this.el=document.getElementById("editMain");const r=t.el.getAttribute("data-endpoint-get"),i=t.el.getAttribute("data-endpoint-save"),s=t.el.getAttribute("data-method");e({data:()=>({euroFreightSettings:[],usFreightSettings:[],formErrorMessage:"",showSuccess:!1,errors:[]}),mounted(){this.populateForm()},methods:{populateForm(){let e=this;fetch(r,{method:s,headers:{"Content-Type":"application/json"}}).then((e=>e.json())).then((function(t){e.euroFreightSettings=t.euroFreightSettings,e.usFreightSettings=t.usFreightSettings}))},validiteForm(){this.errors=[];let e=!0;const t=this.euroFreightSettings.some((e=>isNaN(e.weightInKg)||isNaN(e.marginInDecimal)||isNaN(e.auFreightPerKg)||isNaN(e.nzFreightPerKg)||isNaN(e.auFreightSurcharge)||isNaN(e.nzFreightSurcharge))),r=this.usFreightSettings.some((e=>isNaN(e.weightInKg)||isNaN(e.marginInDecimal)||isNaN(e.auFreightPerKg)||isNaN(e.nzFreightPerKg)||isNaN(e.auFreightSurcharge)||isNaN(e.nzFreightSurcharge)));return(t||r)&&(e=!1,this.errors.push("All numeric fields needs to be a decimal number")),e},saveSettings(){if(this.showSuccess=!1,!this.validiteForm())return;this.formErrorMessage="";let e=this,t={euroFreightSettings:e.euroFreightSettings,usFreightSettings:e.usFreightSettings};fetch(i,{method:s,headers:{"Content-Type":"application/json"},body:"GET"==s?null:JSON.stringify(t),data:"GET"!=s?null:JSON.stringify(t)}).then((e=>e.json())).then((function(t){t.success?e.showSuccess=!0:e.formErrorMessage=t.message}))}}}).mount("#editMain")}})()})();