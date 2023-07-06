import "../../scripts/global"

var Page = new Object()

function SetSettings(){
    const { createApp } = Vue
    let thisObject = this;
    this.el = document.getElementById("editMain");

    const endpointGet = thisObject.el.getAttribute("data-endpoint-get")
    const endpointSave = thisObject.el.getAttribute("data-endpoint-save")
    const method = thisObject.el.getAttribute("data-method")

    createApp({
        data() {
            return {
                euroFreightSettings:[],
                usFreightSettings:[],
                formErrorMessage: '',
                showSuccess:false,
                errors:[]
            }
        },
        mounted(){

                this.populateForm();
            
        },
        methods:{
            
            populateForm(){
                let self = this;
                fetch(endpointGet, {
                    method: method,
                    headers:{
                        'Content-Type': 'application/json'  
                    }
                }).then(res=>res.json())
                    .then(function (response) {

                        self.euroFreightSettings = response.euroFreightSettings;
                        self.usFreightSettings = response.usFreightSettings;

                        if (self.euroFreightSettings.length){
                            self.euroFreightSettings.forEach(element => {
                                element.marginInDecimal = element.marginInDecimal * 100 //display in admin is percentage
                            });
                        }

                        if (self.usFreightSettings.length){
                            self.usFreightSettings.forEach(element => {
                                element.marginInDecimal = element.marginInDecimal * 100 //display in admin is percentage
                            });
                        }
                    });
            },
            validiteForm(){

                this.errors = []
                let validate = true

                const euroError = this.euroFreightSettings.some(p => isNaN(p.weightInKg)
                  || isNaN(p.marginInDecimal) 
                  || isNaN(p.auFreightPerKg)
                  || isNaN(p.nzFreightPerKg)
                  || isNaN(p.auFreightSurcharge)
                  || isNaN(p.nzFreightSurcharge));

                const usError = this.usFreightSettings.some(p => isNaN(p.weightInKg)
                  || isNaN(p.marginInDecimal) 
                  || isNaN(p.auFreightPerKg)
                  || isNaN(p.nzFreightPerKg)
                  || isNaN(p.auFreightSurcharge)
                  || isNaN(p.nzFreightSurcharge));

                if (euroError || usError){

                    validate = false
                    this.errors.push('All numeric fields needs to be a decimal number')
                }
                return validate
            },
            saveSettings(){

                this.showSuccess = false
                if (!this.validiteForm()){
                    return
                }
                this.formErrorMessage = ''
                let self = this;
                let data = {
                    euroFreightSettings:self.euroFreightSettings,
                    usFreightSettings:self.usFreightSettings
                }

                /*if (data.euroFreightSettings.length){
                    data.euroFreightSettings.forEach(element => {
                        element.marginInDecimal = element.marginInDecimal / 100 //convert back to decimal
                    });
                }

                if (data.usFreightSettings.length){
                    data.usFreightSettings.forEach(element => {
                        element.marginInDecimal = element.marginInDecimal / 100 //convert back to decimal
                    });
                }*/

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


Page.SetSettings = new SetSettings();


