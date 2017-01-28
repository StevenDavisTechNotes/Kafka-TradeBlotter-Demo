'use strict';

var express = require('express');
var serveIndex = require('serve-index');
var app = module.exports = express();
app.use(express.static(__dirname));
app.use('/',serveIndex(__dirname, {'icons': true}));

app.listen(8080, function () {
  console.log('Example app listening on port 8080!')
})