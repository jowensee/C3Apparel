(()=>{var t={548:()=>{document.addEventListener("DOMContentLoaded",(function(){document.addEventListener("click",(function(t){"modal-close"==t.target.id&&(t.preventDefault(),document.querySelector(".modal").classList.remove("is-active"))}),!1)}))}},e={};function n(o){var r=e[o];if(void 0!==r)return r.exports;var i=e[o]={exports:{}};return t[o](i,i.exports,n),i.exports}n.n=t=>{var e=t&&t.__esModule?()=>t.default:()=>t;return n.d(e,{a:e}),e},n.d=(t,e)=>{for(var o in e)n.o(e,o)&&!n.o(t,o)&&Object.defineProperty(t,o,{enumerable:!0,get:e[o]})},n.o=(t,e)=>Object.prototype.hasOwnProperty.call(t,e),(()=>{"use strict";n(548),(new Object).SetSettings=new function(){const{createApp:t}=Vue;let e=this;this.el=document.getElementById("editMain");const n=e.el.getAttribute("data-endpoint-get"),o=e.el.getAttribute("data-endpoint-save"),r=e.el.getAttribute("data-method");t({data:()=>({euroFreightSettings:[],usFreightSettings:[],formErrorMessage:""}),mounted(){this.populateForm()},methods:{populateForm(){console.log("endpointGet",n);let t=this;fetch(n,{method:r,headers:{"Content-Type":"application/json"}}).then((t=>t.json())).then((function(e){t.euroFreightSettings=e.euroFreightSettings,t.usFreightSettings=e.usFreightSettings}))},saveSettings(){this.formErrorMessage="";let t=this,e={euroFreightSettings:t.euroFreightSettings,usFreightSettings:t.usFreightSettings};fetch(o,{method:r,headers:{"Content-Type":"application/json"},body:"GET"==r?null:JSON.stringify(e),data:"GET"!=r?null:JSON.stringify(e)}).then((t=>t.json())).then((function(e){e.success,t.formErrorMessage=e.message}))}}}).mount("#editMain")}})()})();