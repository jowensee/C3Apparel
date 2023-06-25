import "../../scripts/global"

var Page = new Object()

function UploadPricingsPage(){
    const { createApp } = Vue
    let thisObject = this;
    this.el = document.getElementById("main-app");

    const endpointUpload = thisObject.el.getAttribute("data-endpoint-upload")
    const method = thisObject.el.getAttribute("data-method")
    const fileInput = document.getElementById('file')
    createApp({
        data() {
            return {
                form:{
                    brandId:0,
                    file: null,
                    deleteAll: false
                },
                validation:{
                    supplier:true,
                    file:true
                },
                formErrorMessage:''
                    
            }
        },
        mounted(){
            
        },
        methods:{
            validate(){

                this.validation.supplier = true
                this.validation.file = true

                let hasError = false;
                if (this.form.brandId == 0){
                    this.validation.supplier = false
                    hasError = true
                }

                if (this.$refs.file.files.length == 0){
                    this.validation.file = false
                    hasError = true
                }

                return !hasError
            },
            upload(){
                
                if (!this.validate()){
                    return
                }
                let self = this;
                let data = {
                    brandId:self.form.brandId,
                    deleteAll:self.form.deleteAll
                }

                const formData = new FormData()
                for (const key in data) {
                    if (data.hasOwnProperty(key)) {
                        formData.append(key, data[key])
                    }
                }
                
                if (this.$refs.file.files.length > 0) {
                    formData.append('file', this.$refs.file.files[0])
                } else {
                    formData.append('file', null)
                }

                console.log(data)
                fetch(endpointUpload, {
                    method: method,
                    body: formData,
                }).then(res=>res.json())
                    .then(function (response) {

                        self.formErrorMessage = response.message

                    });
            }
          
        }
    }).mount('#main-app')

}


Page.UploadPricingsPage = new UploadPricingsPage();



