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
            { security: "FEYE", sodAmount: 4000, sodPrice:100.00 },
            { security: "EXAS", sodAmount: 2000, sodPrice:200.00 },
            { security: "TSLA", sodAmount: 1000, sodPrice:400.00 }];
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
  let messages =[
            JSON.stringify({ security: "FEYE", fillAmount: 400, fillPrice:Number((100.00*(1+0.0101*((count+1)%10))).toFixed(2)) }),
            JSON.stringify({ security: "EXAS", fillAmount: 200, fillPrice:Number((200.00*(1+0.0101*((count+2)%10))).toFixed(2)) }),
            JSON.stringify({ security: "TSLA", fillAmount: 100, fillPrice:Number((400.00*(1+0.0101*((count+3)%10))).toFixed(2)) })];
  ++fillCount;
  console.log('sending Execution');
  console.log(messages);
  producer.send([
    { topic: 'Execution', messages: messages }
  ], function (err, data) {
    if (err) console.log(err);
    //else console.log('sent Executions %d', ++rets);
  });
}

var quoteCount = 0;
function sendQuotes() {
  let count = quoteCount;
  let message =[
            { security: "FEYE", bidPrice:Number((100.00*(1+0.0101*((count+1)%10))).toFixed(2)) },
            { security: "EXAS", bidPrice:Number((200.00*(1+0.0101*((count+2)%10))).toFixed(2)) },
            { security: "TSLA", bidPrice:Number((400.00*(1+0.0101*((count+3)%10))).toFixed(2)) }];
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
