var kafka = require('kafka-node');
var HighLevelProducer = kafka.HighLevelProducer;
var Client = kafka.Client;
var client = new Client('SparkTest:2181');
var argv = require('optimist').argv;
var rets = 0;
var producer = new HighLevelProducer(client);

producer.on('ready', function () {
  producer.createTopics(['SodHoldings', 'Execution', 'Quotes'], false, (error, data) => {
    if (error) {
      console.log('Error creating topic', error)
      process.exit()
    } else {
      console.log('created topics');
    }
  });
  sendSodHoldings();
  setInterval(sendExecutions, 900);
  setInterval(sendQuotes, 700);
});

producer.on('error', function (err) {
  console.log('error', err);
});

function sendSodHoldings() {
  var message =[
            { Security: "SEDOL1", SodAmount: 4000, SodPrice:100.00, PurchaseDate: '2015-12-31', Custodian: 'GSCO', CostBasis: 400*100*3/4 },
            { Security: "SEDOL2", SodAmount: 2000, SodPrice:200.00, PurchaseDate: '2016-06-15', Custodian: 'GSCO', CostBasis: 200*200*3/4 },
            { Security: "SEDOL3", SodAmount: 1000, SodPrice:400.00, PurchaseDate: '2017-01-04', Custodian: 'GSCO', CostBasis: 100*400*3/4 }];
  console.log('sending SodHoldings');
  console.log(message);
  producer.send([
    { topic: 'SodHoldings', messages: [JSON.stringify(message)] }
  ], function (err, data) {
    if (err) console.log(err);
    else console.log('sent SodHoldings');
  });
}

var fillCount = 0;
function sendExecutions() {
  let count = fillCount;
  let now = (new Date()).toISOString();
  let messages =[
            JSON.stringify({ Security: "SEDOL1", FillAmount: 400, FillPrice:Number((100.00*(1+0.0101*((count+1)%10))).toFixed(2)), PurchaseDate:now, Custodian: 'GSCO', ExecutingBroker: 'ACAP' }),
            JSON.stringify({ Security: "SEDOL2", FillAmount: 200, FillPrice:Number((200.00*(1+0.0101*((count+2)%10))).toFixed(2)), PurchaseDate:now, Custodian: 'GSCO', ExecutingBroker: 'ACAP' }),
            JSON.stringify({ Security: "SEDOL3", FillAmount: 100, FillPrice:Number((400.00*(1+0.0101*((count+3)%10))).toFixed(2)), PurchaseDate:now, Custodian: 'GSCO', ExecutingBroker: 'ACAP' })];
  ++fillCount;
  console.log('sending Execution');
  console.log(messages);
  messages.forEach((execution)=> {
    producer.send([
      { topic: 'Execution', messages: [execution] }
    ], function (err, data) {
      if (err) console.log(err);
      //else console.log('sent Executions %d', ++rets);
    });
  });
}

var quoteCount = 0;
function sendQuotes() {
  let count = quoteCount;
  let now = (new Date()).toISOString();
  let message =[
            { Security: "SEDOL1", QuotePrice:Number((100.00*(1+0.0101*((count+1)%10))).toFixed(2)), QuoteDate: now },
            { Security: "SEDOL2", QuotePrice:Number((200.00*(1+0.0101*((count+2)%10))).toFixed(2)), QuoteDate: now },
            { Security: "SEDOL3", QuotePrice:Number((400.00*(1+0.0101*((count+3)%10))).toFixed(2)), QuoteDate: now }];
  ++quoteCount;
  console.log('sending Quotes');
  console.log(message);
  producer.send([
    { topic: 'Quotes', messages: [JSON.stringify(message)] }
  ], function (err, data) {
    if (err) console.log(err);
    //else console.log('sent Executions %d', ++rets);
  });
}
