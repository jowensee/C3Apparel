import { filter } from "lodash";
import "../../scripts/global"

var Page = new Object()

function InternalInquiry(){
    const { createApp } = Vue
    let thisObject = this;
    this.el = document.getElementById("main-app");
    this.itemsPerPage = Number(this.el.getAttribute("data-item-per-page"))
    const listEndpoint = thisObject.el.getAttribute("data-endpoint")
    const initialEndpoint = thisObject.el.getAttribute("data-endpoint-initial")
    const downloadEndpoint = thisObject.el.getAttribute("data-endpoint-download")
    const method = thisObject.el.getAttribute("data-method")

    createApp({
        data() {
            return {
                filter: {
                    targetCurrency: "AUD",
                    brandId: "",
                    brandName: "",
                    collection: "",
                    freightActive:"euro",
                    show: false
                },
                settings:{
                    rateAuUsd:0,
                    rateAuEuro:0,
                    rateNzUsd:0,
                    rateNzEuro:0,
                    dutyAU:0,
                    dutyNZ:0,
                    euroFreightSettings:[],
                    usFreightSettings:[],
                    id: null
                },
                grid: {
                    pagination:{
                        currentPage:1,
                        totalPage:0,
                        rowPerPage:20
                    },
                    rows:[]
                },
                errorMessage: null
            }
        },
        mounted(){
            
            var _this = this;
            fetch(initialEndpoint, {
                method: "GET",
                headers:{
                    'Content-Type': 'application/json'
                }
            }).then(res=>res.json())
                .then(function (response) {
                    _this.settings.dutyAU = response.auImportDuty;
                    _this.settings.dutyNZ = response.nzImportDuty;
                    _this.settings.rateAuUsd = response.rateAuUsd;
                    _this.settings.rateAuEuro = response.rateAuEuro;
                    _this.settings.rateNzUsd = response.rateNzUsd;
                    _this.settings.rateNzEuro = response.rateNzEuro;
                    _this.settings.euroFreightSettings = response.euroFreightSettings;
                    _this.settings.usFreightSettings = response.usFreightSettings;
                });
            
        },
        methods:{
            brand_onChanged(e){
                this.filter.brandName = e.target.options[e.target.options.selectedIndex].text;
                
            },
            toggleFilter(){
                this.filter.show = !this.filter.show
            },
            validateFilter(){
                
                if (this.filter.brandId == ""){
                    this.errorMessage = "Please select supplier"
                    return false;
                }
                return true
            },
            search(){
               this.errorMessage = ''
                if (!this.validateFilter()){
                    return;
                }
              this.populateGrid(1, true)  
            },
            download(){
                this.errorMessage = ''
                if (!this.validateFilter()){
                    return;
                }

                let _this = this;
                fetch(downloadEndpoint + "?id=" + this.settings.id, {
                    method: "GET",
                    headers:{
                        'Content-Type': 'application/json'
                    },
                }).then( res => res.blob() )
                    .then( blob => {
                        var url = window.URL.createObjectURL(blob);
                        var a = document.createElement('a');
                        a.href = url;
                        a.download = `productpricing_${_this.filter.brandName}-${_this.filter.targetCurrency}.csv`;
                        document.body.appendChild(a); // we need to append the element to the dom -> otherwise it will not work in firefox
                        a.click();
                        a.remove();  
                    });
                   /* .then(function (response) {

                       console.log('response', response)

                    });*/

            },
            switchFreightTab(activeTab){
                this.filter.freightActive = activeTab
            },
            populateGrid(pageNumber, saveButtonClicked){
                
                let data = {
                    saveButtonClicked: saveButtonClicked,
                    brandID:this.filter.brandId,
                    targetCurrency:this.filter.targetCurrency,
                    collection:this.filter.collection,
                    pricingSettings:this.settings,
                    pageNumber:pageNumber,
                    itemsPerPage:thisObject.itemsPerPage

                }
                
                let _this = this;
                let grid = this.grid;
                fetch(listEndpoint, {
                    method: method,
                    headers:{
                        'Content-Type': 'application/json'
                    },
                    body: method == "GET" ? null : JSON.stringify(data),
                    data: method != "GET" ? null : JSON.stringify(data),
                }).then(res=>res.json())
                    .then(function (response) {
                        
                        if (response.errorMessage !== null && response.errorMessage !== ""){
                            _this.errorMessage = response.errorMessage
                        }else{

                            _this.settings.id = response.settingsGuid
                            grid.pagination.totalPage = response.totalPage;
                            if (response.pricings !== undefined)
                            {
                                grid.rows = []
                                response.pricings.forEach(element => {
                                    grid.rows.push(element)
                                });
                            }    
                        }
                        
                    });

            },
            gotoPage(event){
                this.grid.pagination.currentPage = Number(event.srcElement.value);
                this.populateGrid(this.grid.pagination.currentPage, false)
            },
            gotoPreviousPage(event){
                event.preventDefault();
                if (this.grid.pagination.currentPage > 1){
                    this.grid.pagination.currentPage--;
                    this.populateGrid(this.grid.pagination.currentPage, false)
                }

            },
            gotoNextPage(event){
                event.preventDefault();
                if (this.grid.pagination.currentPage < this.grid.pagination.totalPage){
                    this.grid.pagination.currentPage++;
                    this.populateGrid(this.grid.pagination.currentPage, false)
                }
            }
        }
    }).mount('#main-app')
}

document.addEventListener("DOMContentLoaded", function(){

    Page.InternalInquiry = new InternalInquiry();
})
