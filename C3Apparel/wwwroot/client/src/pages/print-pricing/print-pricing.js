import "../../scripts/global"

var Page = new Object()

function PrintPricingsPage(){
    const { createApp } = Vue
    let thisObject = this;
    this.el = document.getElementById("main-app");


    createApp({
        data() {
            return {
                form:{
                    brandId:0,
                    currency: ''
                },
                validation:{
                    supplier:true,
                    currency:true
                },
                formErrorMessage:''
                    
            }
        },
        mounted(){
            
        },
        methods:{
            validate(){

                this.validation.supplier = true
                this.validation.currency = true

                let hasError = false;
                if (this.form.brandId == 0){
                    this.validation.supplier = false
                    hasError = true
                }

                if (this.form.currency == ''){
                    this.validation.currency = false
                    hasError = true
                }

                return !hasError
            },
            print(){
                if (!this.validate()){
                    return
                }
                location = `/print-pricing?brandid=${this.form.brandId}&currency=${this.form.currency}`
                
            }
          
        }
    }).mount('#main-app')

}


Page.PrintPricingsPage = new PrintPricingsPage();



