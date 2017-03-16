var kafka = require('kafka-node');
var HighLevelProducer = kafka.HighLevelProducer;
var Client = kafka.Client;
var client = new Client('ZookeeperHost:2181');
var argv = require('optimist').argv;
var rets = 0;
var producer = new HighLevelProducer(client);

producer.on('ready', function () {
  producer.createTopics(['SodHoldings', 'Execution', 'Quotes', 'Exposures', 'RtPositions'], false, function (error, data) {
    if (error) {
      console.log('Error creating topic', error)
      process.exit()
    } else {
      console.log('created topics');
    }
  });
  sendSodHoldings();
  setInterval(sendQuotes, 100);
  setInterval(sendExecutions, 900);
  setInterval(sendSodHoldings, 10*900);
});

producer.on('error', function (err) {
  console.log('error', err);
});

var currentTargetAmount = 4000;
var dayCount = 0;
function sendSodHoldings() {
  dayCount++;
  var message =JSON.stringify(
    {
      "Type": "SodHoldings", Data: [
        { Security: "SEDOL1", SodAmount: currentTargetAmount / 1 * 10, SodPrice: 100.00, CostBasis: 400 * 100 * 3 / 4, TradingDay: dayCount },
        { Security: "SEDOL2", SodAmount: currentTargetAmount / 2 * 10, SodPrice: 200.00, CostBasis: 200 * 200 * 3 / 4, TradingDay: dayCount },
        { Security: "SEDOL3", SodAmount: currentTargetAmount / 4 * 10, SodPrice: 400.00, CostBasis: 100 * 400 * 3 / 4, TradingDay: dayCount }]
        // { Security: "SEDOL1", SodAmount: currentTargetAmount / 1 * 10, TargetAmount: currentTargetAmount / 1, SodPrice: 100.00, PurchaseDate: '2015-12-31', Custodian: 'GSCO', CostBasis: 400 * 100 * 3 / 4, TradingDay: dayCount },
        // { Security: "SEDOL2", SodAmount: currentTargetAmount / 2 * 10, TargetAmount: currentTargetAmount / 2, SodPrice: 200.00, PurchaseDate: '2016-06-15', Custodian: 'GSCO', CostBasis: 200 * 200 * 3 / 4, TradingDay: dayCount },
        // { Security: "SEDOL3", SodAmount: currentTargetAmount / 4 * 10, TargetAmount: currentTargetAmount / 4, SodPrice: 400.00, PurchaseDate: '2017-01-04', Custodian: 'GSCO', CostBasis: 100 * 400 * 3 / 4, TradingDay: dayCount }]
    });
  console.log('sending SodHoldings', dayCount, message.substring(0,80));
  currentTargetAmount += 10 * 400;
  producer.send([
    { topic: 'SodHoldings', messages: [message] }
  ], function (err, data) {
    if (err) console.log(err);
    //else console.log('sent SodHoldings');
  });
}

var fillCount = 0;
function sendExecutions() {
  var count = fillCount;
  var now = (new Date()).toISOString();
  var messages = [
    JSON.stringify({ "Type": "Execution", Data: { Security: "SEDOL1", FillAmount: 400, FillPrice: Number((100.00 * (1 + 0.00101 * ((count + 1) % 10))).toFixed(2)), PurchaseDate: now, TradingDay: dayCount } }),
    JSON.stringify({ "Type": "Execution", Data: { Security: "SEDOL2", FillAmount: 200, FillPrice: Number((200.00 * (1 + 0.00101 * ((count + 2) % 10))).toFixed(2)), PurchaseDate: now, TradingDay: dayCount } }),
    JSON.stringify({ "Type": "Execution", Data: { Security: "SEDOL3", FillAmount: 100, FillPrice: Number((400.00 * (1 + 0.00101 * ((count + 3) % 10))).toFixed(2)), PurchaseDate: now, TradingDay: dayCount } })];
  ++fillCount;
  messages.forEach((message) => {
  console.log('sending Execution', dayCount, fillCount, message.substring(0,80));
    producer.send([
      { topic: 'Execution', messages: [message] }
    ], function (err, data) {
      if (err) console.log(err);
      //else console.log('sent Executions %d', ++rets);
    });
  });
}

var quoteCount = 0;
function sendQuotes() {
  var count = quoteCount;
  var now = (new Date()).toISOString();
  var message = JSON.stringify({
    "Type": "Quotes", Data: [
      { Security: "SEDOL1", QuotePrice: Number((100.00 *1.1* (1 + 0.00101 * ((count + 1) % 10))).toFixed(2)), QuoteDate: now },
      { Security: "SEDOL2", QuotePrice: Number((200.00 *1.1* (1 + 0.00101 * ((count + 2) % 10))).toFixed(2)), QuoteDate: now },
      { Security: "SEDOL3", QuotePrice: Number((400.00 *1.1* (1 + 0.00101 * ((count + 3) % 10))).toFixed(2)), QuoteDate: now }]
  });
  ++quoteCount;
  console.log('sending Quotes', quoteCount, message.substring(0,80));
  producer.send([
    { topic: 'Quotes', messages: [message] }
  ], function (err, data) {
    if (err) console.log(err);
    //else console.log('sent Executions %d', ++rets);
  });
}
