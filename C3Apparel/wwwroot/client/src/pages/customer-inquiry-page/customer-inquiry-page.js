import "../../scripts/global"

var Page = new Object()

function PriceListing(){
    const { createApp } = Vue
    let thisObject = this;
    this.el = document.getElementById("main-app");
    this.currency =  this.el.getAttribute("data-currency") ;
    this.itemsPerPage = Number(this.el.getAttribute("data-item-per-page"))
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
                },
                filter:{
                    brandId:0,
                    c3Style:'',
                    collection:'',
                    description:'',
                    colourDesc:'',
                    productGroup:'',
                    sizes:'',
                    colour:''
                }
            }
        },
        mounted(){
            
        },
        methods:{
            populateGrid(pageNumber){
                console.log('populate')
                let data = {
                    filters:{
                        brandId: this.filter.brandId,
                        c3Style:this.filter.c3Style,
                        collection:this.filter.collection,
                        description:this.filter.description,
                        productGroup:this.filter.productGroup,
                        sizes:this.filter.sizes,
                        colours:this.filter.colour,
                        colourDescriptions:this.filter.colourDesc
                     },
                    currency:thisObject.currency,
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
            },filterResults(){
                this.populateGrid(1);
            },
        }
    }).mount('#main-app')
}

document.addEventListener("DOMContentLoaded", function(){


    Page.PriceListing = new PriceListing();
})
