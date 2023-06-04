import "../../scripts/global"

var Page = new Object()

function BrandListing(){
    const { createApp } = Vue
    let thisObject = this;
    this.el = document.getElementById("main-app");
    this.filterName = '';
    this.filterFocus = '';
    this.filterCurrency = '';
    this.itemsPerPage = Number(this.el.getAttribute("data-item-per-page"))
    if (document.getElementById("filterName").value != ''){
        this.filterName = document.getElementById("filterName").value
    }
    if (document.getElementById("filterFocus").value != ''){
        this.filterFocus = document.getElementById("filterFocus").value
    }
    if (document.getElementById("filterCurrency").value != ''){
        this.filterCurrency = document.getElementById("filterCurrency").value
    }
    const endpoint = thisObject.el.getAttribute("data-endpoint")
    const deleteEndpoint = thisObject.el.getAttribute("data-delete-endpoint")
    const method = thisObject.el.getAttribute("data-method")

    createApp({
        data() {
            return {
                showDeletePopup:false,
                deleteId:0,
                grid: {
                    pagination:{
                        currentPage:1,
                        totalPage:0,
                        rowPerPage:20
                    },
                    rows:[]
                }
            }
        },
        mounted(){
            
            this.populateGrid(1);
            
            
        },
        methods:{
            populateGrid(pageNumber){
                let data = {
                    filters:{
                       filterName: this.filterName,
                        filterFocus:this.filterFocus,
                        filterCurrency:this.filterCurrency

                    },
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
                        if (response.brands !== undefined)
                        {
                            grid.rows = []
                            response.brands.forEach(element => {
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
            loadDeleteConfirmation(id){

                this.showDeletePopup = true
                this.deleteId = id
            },
            deleteBrand(){
                let data = {
                    id:this.deleteId
                }
                console.log('delete',data)
                fetch(deleteEndpoint, {
                    method: method,
                    headers:{
                        'Content-Type': 'application/json'  
                    },
                    body: method == "GET" ? null : JSON.stringify(data),
                    data: method != "GET" ? null : JSON.stringify(data),
                }).then(res=>res.json())
                    .then(function (response) {
                        if (response.success){
                            location.reload()

                        }else{
                            console.log(response.message)
                        }
                    });
            }
        }
    }).mount('#listingMain')
}

document.addEventListener("DOMContentLoaded", function(){

    document.addEventListener('click', function (event) {

        if (event.target.id != 'buttonSearch') return;

        event.preventDefault();
        let container = document.querySelector(".filter-container");
        container.classList.remove("show-error");
        var brandSelect = document.getElementById("selectBrand");

        if (brandSelect.value == ''){
            

            container.classList.add("show-error");
            container.querySelector(".error").textContent = "Please select a brand";

            return;
        }

        location = "?brandid=" + brandSelect.value
    
    }, false);

    Page.BrandListing = new BrandListing();
})
