import "../../scripts/global"
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import CKEditor from "@ckeditor/ckeditor5-vue"

var Page = new Object()

function BrandEdit(){
    const { createApp } = Vue
    let thisObject = this;
    this.el = document.getElementById("editMain");

    const endpointGet = thisObject.el.getAttribute("data-endpoint-get")
    const endpointSave = thisObject.el.getAttribute("data-endpoint-save")
    const method = thisObject.el.getAttribute("data-method")

    createApp({
        components:{Datepicker: VueDatePicker,
                    ckeditor: CKEditor.component},
        data() {
            return {
                editor: ClassicEditor,
                brand:{
                    brandId:Number(thisObject.el.getAttribute("data-brand-id")),
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
                },
                formErrorMessage:'',
                showSuccess:false,
                errors:[]
                    
            }
        },
        mounted(){
            if (this.brand.brandId > 0){

                this.populateForm(this.brand.brandId);
            }
            
        },
        methods:{
            
            populateForm(brandId){
                let data = {
                    id:brandId,

                }
                let brand = this.brand;
                fetch(endpointGet, {
                    method: method,
                    headers:{
                        'Content-Type': 'application/json'  
                    },
                    body: method == "GET" ? null : JSON.stringify(data),
                    data: method != "GET" ? null : JSON.stringify(data),
                }).then(res=>res.json())
                    .then(function (response) {

                        brand.brandId = response.brand.brandId
                        brand.name = response.brand.brand
                        brand.focus = response.brand.focus
                        brand.currency = response.brand.currency
                        brand.description = response.brand.description
                        brand.website = response.brand.website
                        brand.businessName = response.brand.businessName
                        brand.disclaimerAU = response.brand.disclaimerTextAU
                        brand.disclaimerNZ = response.brand.disclaimerTextNZ
                        brand.enabled = response.brand.enabled
                        brand.publishDate = response.brand.publishDate
                    });
            },
            validiteForm(){

                this.errors = []
                let validate = true

                if (this.brand.name.trim() == ''){
                    this.errors.push("<b>Display name:</b> Please enter a value")
                    validate = false
                }
                return validate
            },
            saveBrand(){
                
                this.showSuccess = false
                if (!this.validiteForm()){
                    return
                }
                
                let self = this;
                let data = {
                    brandId:self.brand.brandId,
                    brand:self.brand.name,
                    focus:self.brand.focus,
                    currency:self.brand.currency,
                    description:self.brand.description,
                    website:self.brand.website,
                    businessName:self.brand.businessName,
                    disclaimerTextAU:self.brand.disclaimerAU,
                    disclaimerTextNZ:self.brand.disclaimerNZ,
                    enabled:self.brand.enabled,
                    publishDate:self.brand.publishDate,
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
                            self.showSuccess = true
                        }else{
                            self.formErrorMessage = response.message
                        }
                    });
            }
          
        }
    }).mount('#editMain')

}


Page.BrandEdit = new BrandEdit();


