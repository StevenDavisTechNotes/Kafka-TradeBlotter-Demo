'use strict';

var kafka = require('kafka-node');
var HighLevelConsumer = kafka.HighLevelConsumer;
var Client = kafka.Client;
var argv = require('optimist').argv;
var topic = argv.topic || 'topic1';
var client = new Client('SparkTest:2181');
// var topics = [{ topic: 'SodHoldings' }, { topic: 'Execution' }, { topic: 'Quotes' }, { topic: 'Exposures' }, { topic: 'RtPositions' }];
var topics = [{ topic: 'RtPositions' }];
var options = { autoCommit: true, fetchMaxWaitMs: 1000, fetchMaxBytes: 1024 * 1024 };
var consumer = new HighLevelConsumer(client, topics, options);

consumer.on('message', function (message) {
  console.log(message);
});

consumer.on('error', function (err) {
  console.log('error', err);
});
