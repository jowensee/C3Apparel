import "../../scripts/global"

var Page = new Object()

function RateEdit(){
    const { createApp } = Vue
    let thisObject = this;
    this.el = document.getElementById("editMain");

    const endpointGet = thisObject.el.getAttribute("data-endpoint-get")
    const endpointSave = thisObject.el.getAttribute("data-endpoint-save")
    const method = thisObject.el.getAttribute("data-method")

    createApp({
        components:{Datepicker: VueDatePicker},
        data() {
            return {
                rate:{
                    id:Number(thisObject.el.getAttribute("data-id")),
                    sourceCurrency:'',
                    audValue:0,
                    nzdValue:0,
                    validFrom:'',
                    validTo:'',
                },
                formErrorMessage:'',
                showSuccess:false,
                errors:[]
                    
            }
        },
        mounted(){
            if (this.rate.id > 0){

                this.populateForm(this.rate.id );
            }
            
        },
        methods:{
            
            populateForm(id){
                let data = {
                    id:id,

                }
                let rate = this.rate;
                fetch(endpointGet, {
                    method: method,
                    headers:{
                        'Content-Type': 'application/json'  
                    },
                    body: method == "GET" ? null : JSON.stringify(data),
                    data: method != "GET" ? null : JSON.stringify(data),
                }).then(res=>res.json())
                    .then(function (response) {
                        rate.id = response.rate.id,
                        rate.sourceCurrency = response.rate.sourceCurrency
                        rate.audValue = response.rate.audValue
                        rate.nzdValue = response.rate.nzdValue
                        rate.validFrom = response.rate.validFrom
                        rate.validTo= response.rate.validTo
                        
                    });
            },
            validiteForm(){

                this.errors = []
                let validate = true

                if (this.rate.sourceCurrency.trim() == ''){
                    this.errors.push("<b>Source Currency:</b> Please select one")
                    validate = false
                }

                if (isNaN(this.rate.audValue)){
                    this.errors.push("<b>AUD Value:</b> Please enter a decimal value")
                    validate = false
                }

                if (isNaN(this.rate.nzdValue)){
                    this.errors.push("<b>NZD Value:</b> Please enter a decimal value")
                    validate = false
                }

                return validate
            },
            saveExchangeRate(){
                
                this.showSuccess = false
                if (!this.validiteForm()){
                    return
                }
                let self = this;
                let data = {
                    id:self.rate.id,
                    sourceCurrency:self.rate.sourceCurrency,
                    audValue:self.rate.audValue,
                    nzdValue:self.rate.nzdValue,
                    validFrom:self.rate.validFrom,
                    validTo: self.rate.validTo
                   
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


Page.RateEdit = new RateEdit();



