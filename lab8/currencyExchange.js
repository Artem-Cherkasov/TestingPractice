const mbHelper = require('./mountebank-helper');
const settings = require('./settings');

function addService() {
    const getСurrenciesResponse = {
        EUR_To_RUB: "74.18, 1", 
        USD_To_RUB: "71.33, 1",
        USD_To_EUR: "1, 0.94",
        USD_To_CNY: "1, 7.04"
    }

    const USD_To_RUB = {
        USD_To_RUB: "71.33, 1"
    }

    const stubs = [
        {
            predicates: [{
                equals: {
                    method: "GET",
                    "path": "/getCurrencyExchange"
                }
            }],
            responses: [
                {
                    is: {
                        statusCode: 200,
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify(getСurrenciesResponse)
                    }
                }
            ] 
        },

        {
            predicates: [{
                equals: {
                    method: "GET",
                    "path": "/getCurrencyExchange/USD_To_RUB"
                }
            }],
            responses: [
                {
                    is: {
                        statusCode: 200,
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify(USD_To_RUB)
                    }
                }
            ] 
        },

        {
            responses: [
                { is: { statusCode: 400 } }
            ]
        }
    ];

    const imposter = {
        port: settings.currency_exchange_service_port,
        protocol: 'http',
        stubs: stubs
    };
    return mbHelper.postImposter(imposter);
}

module.exports = { addService };