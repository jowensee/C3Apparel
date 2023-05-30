import "../../scripts/global"

var Page = new Object()

function PriceListing(){
    const { createApp } = Vue
    let thisObject = this;
    this.el = document.getElementById("listingMain");
    this.selectedBrandID = 0;
    this.itemsPerPage = Number(this.el.getAttribute("data-item-per-page"))
    if (document.getElementById("selectBrand").value != ''){
        this.selectedBrandID = Number(document.getElementById("selectBrand").value)
    }
    const endpoint = thisObject.el.getAttribute("data-endpoint")
    const method = thisObject.el.getAttribute("data-method")

    createApp({
        data() {
            return {
                
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
            
            if (thisObject.selectedBrandID > 0) {
                this.populateGrid(1);
            }
            
        },
        methods:{
            populateGrid(pageNumber){
                let data = {
                    brandID:thisObject.selectedBrandID,
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

    Page.PriceListing = new PriceListing();
})
