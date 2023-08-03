import "../../scripts/global"

var Page = new Object()

function PrintPricingsPage(){
    const { createApp } = Vue
    let thisObject = this;
    this.el = document.getElementById("main-app");
    this.itemsPerPage = Number(this.el.getAttribute("data-item-per-page"))
    const endpoint = thisObject.el.getAttribute("data-endpoint")
    const endpointSave = thisObject.el.getAttribute("data-endpoint-save")
    const method = thisObject.el.getAttribute("data-method")


    createApp({
        data() {
            return {
                form:{
                    brandId:0
                },
                validation:{
                    brand:true
                },
                formErrorMessage: '',
                showSuccess:false,
                grid: {
                    pagination:{
                        currentPage:1,
                        totalPage:0,
                        rowPerPage:20
                    },
                    rows:[]
                },
            }
        },
        mounted(){
            this.populateGrid(1);
        },
        methods:{
            validate(){

                this.validation.brandId = true

                let hasError = false;
                if (this.form.brandId == 0){
                    this.validation.brand = false
                    hasError = true
                }

                return !hasError
            },
            populateGrid(pageNumber){
                let data = {
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
            generatePriceList(brandId){

                this.showSuccess = false
                this.formErrorMessage = ''
                let self = this;
             
                let data = {
                    brandId:brandId,
                }
                fetch(endpointSave, {
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
                            self.formErrorMessage = response.message
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
          
        }
    }).mount('#main-app')

}


Page.PrintPricingsPage = new PrintPricingsPage();



