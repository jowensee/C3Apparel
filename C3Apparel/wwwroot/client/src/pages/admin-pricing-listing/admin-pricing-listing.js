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
    const initialFilter = thisObject.el.getAttribute("data-initial-filter")
    const downloadEndpoint = thisObject.el.getAttribute("data-endpoint-download")

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
                },
                searchClicked:false,
                filterValidationError: '',
                filterBrandName:''

            }
        },
        computed:{
            showError: function(){
                return this.grid.rows.length == 0 && this.searchClicked
            }

        },
        mounted(){
            
            var initialFilterObject = JSON.parse(initialFilter)

            if (initialFilterObject != null){
                this.filter.brandId = initialFilterObject.filters.filterSupplier
                this.filter.c3Style = initialFilterObject.filters.filterC3Style
                this.filter.collection = initialFilterObject.filters.filterCollection
                this.filter.supplierStyle = initialFilterObject.filters.filterSupplierStyle
                this.filter.description = initialFilterObject.filters.filterDescription
                this.filter.coo = initialFilterObject.filters.filterCOO
                this.filter.productGroup = initialFilterObject.filters.filterProductGroup
                this.filter.sizes = initialFilterObject.filters.filterSizes
                this.filter.colour = initialFilterObject.filters.filterColour
            }

            if (this.filter.brandId > 0)
            {
                this.populateGrid(initialFilterObject == null || initialFilterObject.pageNumber == 0 ? 1 :initialFilterObject.pageNumber)
            }
            //this.populateGrid(1);
            
            
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

                if (this.filter.brandId != ''){
                    this.searchClicked = true
                    this.filterValidationError = ''

                    var sel = document.getElementById("filterBrand");
                    this.filterBrandName = sel.options[sel.selectedIndex].text;

                    this.populateGrid(1);
                }else{
                    this.filterBrandName = ''
                    this.filterValidationError = 'Please select a brand.'
                }
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
            },
            download(){
                this.errorMessage = ''
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
                     }
                }

                let _this = this;
                fetch(downloadEndpoint, {
                    method: "POST",
                    headers:{
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(data),
                    data: null
                }).then( res => res.blob() )
                    .then( blob => {
                        var url = window.URL.createObjectURL(blob);
                        var a = document.createElement('a');
                        a.href = url;
                        a.download = `C-3_admin_products_${this.filterBrandName}.csv`;
                        document.body.appendChild(a); // we need to append the element to the dom -> otherwise it will not work in firefox
                        a.click();
                        a.remove();  
                    });
                   /* .then(function (response) {

                       console.log('response', response)

                    });*/

            },
        }
    }).mount('#main-app')
}

document.addEventListener("DOMContentLoaded", function(){

    Page.PricingListing = new PricingListing();
})
