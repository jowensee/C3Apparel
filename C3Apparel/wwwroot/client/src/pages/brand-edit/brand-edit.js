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
    const endpointGet = thisObject.el.getAttribute("data-endpoint-get")
    const endpointSave = thisObject.el.getAttribute("data-endpoint-save")
    const method = thisObject.el.getAttribute("data-method")

    createApp({
        data() {
            return {
                brand:{
                    brandId:0,
                    name:'',
                    focus:'',
                    currency:'',
                    description:'',
                    website:'',
                    businessName:'',
                    disclaimerAU:'',
                    disclaimerNZ:'',
                    enabled:'',
                    publishDate:'',
                }
                    
            }
        },
        mounted(){
            
            this.populateGrid(1);
            
            
        },
        methods:{

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
    }).mount('#editMain')
}


