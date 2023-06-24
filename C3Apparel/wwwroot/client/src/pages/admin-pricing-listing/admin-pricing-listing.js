import "../../scripts/global"

var Page = new Object()

function PricingListing(){
    const { createApp } = Vue
    let thisObject = this;
    this.el = document.getElementById("main-app");
    
    this.itemsPerPage = Number(this.el.getAttribute("data-item-per-page"))
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
                    brandId:0,
                    c3Style:'',
                    collection:'',
                    supplierStyle:'',
                    description:'',
                    coo:'',
                    productGroup:'',
                    sizes:'',
                    colour:''
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
                       filterSupplier: this.filter.brandId,
                       filterC3Style:this.filter.c3Style,
                       filterCollection:this.filter.collection,
                       filterSupplierStyle:this.filter.supplierStyle,
                       filterDescription:this.filter.description,
                       FilterCOO:this.filter.coo,
                       filterProductGroup:this.filter.productGroup,
                       filterSizes:this.filter.sizes,
                       filterColour:this.filter.colour
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
                        if (response.pricings !== undefined)
                        {
                            grid.rows = []
                            response.pricings.forEach(element => {
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
            deleteProductPrice(){
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

    Page.PricingListing = new PricingListing();
})
