import "../../scripts/global"

var Page = new Object()

function BrandListing(){
    const { createApp } = Vue
    let thisObject = this;
    this.el = document.getElementById("main-app");
    
    this.itemsPerPage = Number(this.el.getAttribute("data-item-per-page"))
    /*this.filterName = '';
    this.filterFocus = '';
    this.filterCurrency = '';
    if (document.getElementById("filterName").value != ''){
        this.filterName = document.getElementById("filterName").value
    }
    if (document.getElementById("filterFocus").value != ''){
        this.filterFocus = document.getElementById("filterFocus").value
    }
    if (document.getElementById("filterCurrency").value != ''){
        this.filterCurrency = document.getElementById("filterCurrency").value
    }*/
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
                },
                filter:{
                    name:'',
                    focus:'',
                    currency:''
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
                       filterName: this.filter.name,
                        filterFocus:this.filter.focus,
                        filterCurrency:this.filter.currency

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
            filterResults(){
                console.log('filterresults')
                this.populateGrid(1);
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
    }).mount('#main-app')
}

document.addEventListener("DOMContentLoaded", function(){

    Page.BrandListing = new BrandListing();
})
