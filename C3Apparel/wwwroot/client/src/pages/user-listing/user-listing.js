import "../../scripts/global"

var Page = new Object()

function UserListing(){
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
                deleteId:'',
                grid: {
                    pagination:{
                        currentPage:1,
                        totalPage:0,
                        rowPerPage:20
                    },
                    rows:[]
                },
                filter:{
                    userName:'',
                    role:''
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
                       filterUserName: this.filter.userName,
                        filterRole:this.filter.role
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
                        if (response.users !== undefined)
                        {
                            grid.rows = []
                            response.users.forEach(element => {
                                grid.rows.push(element)
                            });
                        }
                    });

            },
            filterResults(){
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
            deleteUser(){
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
            }
        }
    }).mount('#main-app')
}

document.addEventListener("DOMContentLoaded", function(){

    Page.UserListing = new UserListing();
})
