const mb = require('mountebank');
const settings = require('./settings');
const currencyExchangeService = require('./currencyExchange');

const mbServerInstance = mb.create({
    port: settings.port,
    pidfile: '../mb.pid',
    logfile: '../mb.log',
    protofile: '../protofile.json',
    ipWhitelist: ['*'] 
});

mbServerInstance.then(function() {
    currencyExchangeService.addService();   
});