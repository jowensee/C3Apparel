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
                formErrorMessage: ''
            }
        },
        mounted(){

                this.populateForm();
            
        },
        methods:{
            
            populateForm(){
                console.log('endpointGet',endpointGet)
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
                    });
            },
            saveSettings(){
                
                this.formErrorMessage = ''
                let self = this;
                let data = {
                    euroFreightSettings:self.euroFreightSettings,
                    usFreightSettings:self.usFreightSettings
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
                            self.formErrorMessage = response.message
                        }else{
                            self.formErrorMessage = response.message
                        }
                    });
            }
          
        }
    }).mount('#editMain')

}


Page.SetSettings = new SetSettings();


