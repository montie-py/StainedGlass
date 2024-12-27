/*
 * ATTENTION: The "eval" devtool has been used (maybe by default in mode: "development").
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
/******/ (() => { // webpackBootstrap
/******/ 	var __webpack_modules__ = ({

/***/ "./node_modules/es6-promise/auto.js":
/*!******************************************!*\
  !*** ./node_modules/es6-promise/auto.js ***!
  \******************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {

"use strict";
eval("// This file can be required in Browserify and Node.js for automatic polyfill\n// To use it:  require('es6-promise/auto');\n\nmodule.exports = (__webpack_require__(/*! ./ */ \"./node_modules/es6-promise/dist/es6-promise.js\").polyfill)();\n\n\n//# sourceURL=webpack://stainedglass.web/./node_modules/es6-promise/auto.js?");

/***/ }),

/***/ "./node_modules/es6-promise/dist/es6-promise.js":
/*!******************************************************!*\
  !*** ./node_modules/es6-promise/dist/es6-promise.js ***!
  \******************************************************/
/***/ (function(module, __unused_webpack_exports, __webpack_require__) {

eval("/*!\n * @overview es6-promise - a tiny implementation of Promises/A+.\n * @copyright Copyright (c) 2014 Yehuda Katz, Tom Dale, Stefan Penner and contributors (Conversion to ES6 API by Jake Archibald)\n * @license   Licensed under MIT license\n *            See https://raw.githubusercontent.com/stefanpenner/es6-promise/master/LICENSE\n * @version   v4.2.8+1e68dce6\n */\n\n(function (global, factory) {\n\t true ? module.exports = factory() :\n\t0;\n}(this, (function () { 'use strict';\n\nfunction objectOrFunction(x) {\n  var type = typeof x;\n  return x !== null && (type === 'object' || type === 'function');\n}\n\nfunction isFunction(x) {\n  return typeof x === 'function';\n}\n\n\n\nvar _isArray = void 0;\nif (Array.isArray) {\n  _isArray = Array.isArray;\n} else {\n  _isArray = function (x) {\n    return Object.prototype.toString.call(x) === '[object Array]';\n  };\n}\n\nvar isArray = _isArray;\n\nvar len = 0;\nvar vertxNext = void 0;\nvar customSchedulerFn = void 0;\n\nvar asap = function asap(callback, arg) {\n  queue[len] = callback;\n  queue[len + 1] = arg;\n  len += 2;\n  if (len === 2) {\n    // If len is 2, that means that we need to schedule an async flush.\n    // If additional callbacks are queued before the queue is flushed, they\n    // will be processed by this flush that we are scheduling.\n    if (customSchedulerFn) {\n      customSchedulerFn(flush);\n    } else {\n      scheduleFlush();\n    }\n  }\n};\n\nfunction setScheduler(scheduleFn) {\n  customSchedulerFn = scheduleFn;\n}\n\nfunction setAsap(asapFn) {\n  asap = asapFn;\n}\n\nvar browserWindow = typeof window !== 'undefined' ? window : undefined;\nvar browserGlobal = browserWindow || {};\nvar BrowserMutationObserver = browserGlobal.MutationObserver || browserGlobal.WebKitMutationObserver;\nvar isNode = typeof self === 'undefined' && typeof process !== 'undefined' && {}.toString.call(process) === '[object process]';\n\n// test for web worker but not in IE10\nvar isWorker = typeof Uint8ClampedArray !== 'undefined' && typeof importScripts !== 'undefined' && typeof MessageChannel !== 'undefined';\n\n// node\nfunction useNextTick() {\n  // node version 0.10.x displays a deprecation warning when nextTick is used recursively\n  // see https://github.com/cujojs/when/issues/410 for details\n  return function () {\n    return process.nextTick(flush);\n  };\n}\n\n// vertx\nfunction useVertxTimer() {\n  if (typeof vertxNext !== 'undefined') {\n    return function () {\n      vertxNext(flush);\n    };\n  }\n\n  return useSetTimeout();\n}\n\nfunction useMutationObserver() {\n  var iterations = 0;\n  var observer = new BrowserMutationObserver(flush);\n  var node = document.createTextNode('');\n  observer.observe(node, { characterData: true });\n\n  return function () {\n    node.data = iterations = ++iterations % 2;\n  };\n}\n\n// web worker\nfunction useMessageChannel() {\n  var channel = new MessageChannel();\n  channel.port1.onmessage = flush;\n  return function () {\n    return channel.port2.postMessage(0);\n  };\n}\n\nfunction useSetTimeout() {\n  // Store setTimeout reference so es6-promise will be unaffected by\n  // other code modifying setTimeout (like sinon.useFakeTimers())\n  var globalSetTimeout = setTimeout;\n  return function () {\n    return globalSetTimeout(flush, 1);\n  };\n}\n\nvar queue = new Array(1000);\nfunction flush() {\n  for (var i = 0; i < len; i += 2) {\n    var callback = queue[i];\n    var arg = queue[i + 1];\n\n    callback(arg);\n\n    queue[i] = undefined;\n    queue[i + 1] = undefined;\n  }\n\n  len = 0;\n}\n\nfunction attemptVertx() {\n  try {\n    var vertx = Function('return this')().require('vertx');\n    vertxNext = vertx.runOnLoop || vertx.runOnContext;\n    return useVertxTimer();\n  } catch (e) {\n    return useSetTimeout();\n  }\n}\n\nvar scheduleFlush = void 0;\n// Decide what async method to use to triggering processing of queued callbacks:\nif (isNode) {\n  scheduleFlush = useNextTick();\n} else if (BrowserMutationObserver) {\n  scheduleFlush = useMutationObserver();\n} else if (isWorker) {\n  scheduleFlush = useMessageChannel();\n} else if (browserWindow === undefined && \"function\" === 'function') {\n  scheduleFlush = attemptVertx();\n} else {\n  scheduleFlush = useSetTimeout();\n}\n\nfunction then(onFulfillment, onRejection) {\n  var parent = this;\n\n  var child = new this.constructor(noop);\n\n  if (child[PROMISE_ID] === undefined) {\n    makePromise(child);\n  }\n\n  var _state = parent._state;\n\n\n  if (_state) {\n    var callback = arguments[_state - 1];\n    asap(function () {\n      return invokeCallback(_state, child, callback, parent._result);\n    });\n  } else {\n    subscribe(parent, child, onFulfillment, onRejection);\n  }\n\n  return child;\n}\n\n/**\n  `Promise.resolve` returns a promise that will become resolved with the\n  passed `value`. It is shorthand for the following:\n\n  ```javascript\n  let promise = new Promise(function(resolve, reject){\n    resolve(1);\n  });\n\n  promise.then(function(value){\n    // value === 1\n  });\n  ```\n\n  Instead of writing the above, your code now simply becomes the following:\n\n  ```javascript\n  let promise = Promise.resolve(1);\n\n  promise.then(function(value){\n    // value === 1\n  });\n  ```\n\n  @method resolve\n  @static\n  @param {Any} value value that the returned promise will be resolved with\n  Useful for tooling.\n  @return {Promise} a promise that will become fulfilled with the given\n  `value`\n*/\nfunction resolve$1(object) {\n  /*jshint validthis:true */\n  var Constructor = this;\n\n  if (object && typeof object === 'object' && object.constructor === Constructor) {\n    return object;\n  }\n\n  var promise = new Constructor(noop);\n  resolve(promise, object);\n  return promise;\n}\n\nvar PROMISE_ID = Math.random().toString(36).substring(2);\n\nfunction noop() {}\n\nvar PENDING = void 0;\nvar FULFILLED = 1;\nvar REJECTED = 2;\n\nfunction selfFulfillment() {\n  return new TypeError(\"You cannot resolve a promise with itself\");\n}\n\nfunction cannotReturnOwn() {\n  return new TypeError('A promises callback cannot return that same promise.');\n}\n\nfunction tryThen(then$$1, value, fulfillmentHandler, rejectionHandler) {\n  try {\n    then$$1.call(value, fulfillmentHandler, rejectionHandler);\n  } catch (e) {\n    return e;\n  }\n}\n\nfunction handleForeignThenable(promise, thenable, then$$1) {\n  asap(function (promise) {\n    var sealed = false;\n    var error = tryThen(then$$1, thenable, function (value) {\n      if (sealed) {\n        return;\n      }\n      sealed = true;\n      if (thenable !== value) {\n        resolve(promise, value);\n      } else {\n        fulfill(promise, value);\n      }\n    }, function (reason) {\n      if (sealed) {\n        return;\n      }\n      sealed = true;\n\n      reject(promise, reason);\n    }, 'Settle: ' + (promise._label || ' unknown promise'));\n\n    if (!sealed && error) {\n      sealed = true;\n      reject(promise, error);\n    }\n  }, promise);\n}\n\nfunction handleOwnThenable(promise, thenable) {\n  if (thenable._state === FULFILLED) {\n    fulfill(promise, thenable._result);\n  } else if (thenable._state === REJECTED) {\n    reject(promise, thenable._result);\n  } else {\n    subscribe(thenable, undefined, function (value) {\n      return resolve(promise, value);\n    }, function (reason) {\n      return reject(promise, reason);\n    });\n  }\n}\n\nfunction handleMaybeThenable(promise, maybeThenable, then$$1) {\n  if (maybeThenable.constructor === promise.constructor && then$$1 === then && maybeThenable.constructor.resolve === resolve$1) {\n    handleOwnThenable(promise, maybeThenable);\n  } else {\n    if (then$$1 === undefined) {\n      fulfill(promise, maybeThenable);\n    } else if (isFunction(then$$1)) {\n      handleForeignThenable(promise, maybeThenable, then$$1);\n    } else {\n      fulfill(promise, maybeThenable);\n    }\n  }\n}\n\nfunction resolve(promise, value) {\n  if (promise === value) {\n    reject(promise, selfFulfillment());\n  } else if (objectOrFunction(value)) {\n    var then$$1 = void 0;\n    try {\n      then$$1 = value.then;\n    } catch (error) {\n      reject(promise, error);\n      return;\n    }\n    handleMaybeThenable(promise, value, then$$1);\n  } else {\n    fulfill(promise, value);\n  }\n}\n\nfunction publishRejection(promise) {\n  if (promise._onerror) {\n    promise._onerror(promise._result);\n  }\n\n  publish(promise);\n}\n\nfunction fulfill(promise, value) {\n  if (promise._state !== PENDING) {\n    return;\n  }\n\n  promise._result = value;\n  promise._state = FULFILLED;\n\n  if (promise._subscribers.length !== 0) {\n    asap(publish, promise);\n  }\n}\n\nfunction reject(promise, reason) {\n  if (promise._state !== PENDING) {\n    return;\n  }\n  promise._state = REJECTED;\n  promise._result = reason;\n\n  asap(publishRejection, promise);\n}\n\nfunction subscribe(parent, child, onFulfillment, onRejection) {\n  var _subscribers = parent._subscribers;\n  var length = _subscribers.length;\n\n\n  parent._onerror = null;\n\n  _subscribers[length] = child;\n  _subscribers[length + FULFILLED] = onFulfillment;\n  _subscribers[length + REJECTED] = onRejection;\n\n  if (length === 0 && parent._state) {\n    asap(publish, parent);\n  }\n}\n\nfunction publish(promise) {\n  var subscribers = promise._subscribers;\n  var settled = promise._state;\n\n  if (subscribers.length === 0) {\n    return;\n  }\n\n  var child = void 0,\n      callback = void 0,\n      detail = promise._result;\n\n  for (var i = 0; i < subscribers.length; i += 3) {\n    child = subscribers[i];\n    callback = subscribers[i + settled];\n\n    if (child) {\n      invokeCallback(settled, child, callback, detail);\n    } else {\n      callback(detail);\n    }\n  }\n\n  promise._subscribers.length = 0;\n}\n\nfunction invokeCallback(settled, promise, callback, detail) {\n  var hasCallback = isFunction(callback),\n      value = void 0,\n      error = void 0,\n      succeeded = true;\n\n  if (hasCallback) {\n    try {\n      value = callback(detail);\n    } catch (e) {\n      succeeded = false;\n      error = e;\n    }\n\n    if (promise === value) {\n      reject(promise, cannotReturnOwn());\n      return;\n    }\n  } else {\n    value = detail;\n  }\n\n  if (promise._state !== PENDING) {\n    // noop\n  } else if (hasCallback && succeeded) {\n    resolve(promise, value);\n  } else if (succeeded === false) {\n    reject(promise, error);\n  } else if (settled === FULFILLED) {\n    fulfill(promise, value);\n  } else if (settled === REJECTED) {\n    reject(promise, value);\n  }\n}\n\nfunction initializePromise(promise, resolver) {\n  try {\n    resolver(function resolvePromise(value) {\n      resolve(promise, value);\n    }, function rejectPromise(reason) {\n      reject(promise, reason);\n    });\n  } catch (e) {\n    reject(promise, e);\n  }\n}\n\nvar id = 0;\nfunction nextId() {\n  return id++;\n}\n\nfunction makePromise(promise) {\n  promise[PROMISE_ID] = id++;\n  promise._state = undefined;\n  promise._result = undefined;\n  promise._subscribers = [];\n}\n\nfunction validationError() {\n  return new Error('Array Methods must be provided an Array');\n}\n\nvar Enumerator = function () {\n  function Enumerator(Constructor, input) {\n    this._instanceConstructor = Constructor;\n    this.promise = new Constructor(noop);\n\n    if (!this.promise[PROMISE_ID]) {\n      makePromise(this.promise);\n    }\n\n    if (isArray(input)) {\n      this.length = input.length;\n      this._remaining = input.length;\n\n      this._result = new Array(this.length);\n\n      if (this.length === 0) {\n        fulfill(this.promise, this._result);\n      } else {\n        this.length = this.length || 0;\n        this._enumerate(input);\n        if (this._remaining === 0) {\n          fulfill(this.promise, this._result);\n        }\n      }\n    } else {\n      reject(this.promise, validationError());\n    }\n  }\n\n  Enumerator.prototype._enumerate = function _enumerate(input) {\n    for (var i = 0; this._state === PENDING && i < input.length; i++) {\n      this._eachEntry(input[i], i);\n    }\n  };\n\n  Enumerator.prototype._eachEntry = function _eachEntry(entry, i) {\n    var c = this._instanceConstructor;\n    var resolve$$1 = c.resolve;\n\n\n    if (resolve$$1 === resolve$1) {\n      var _then = void 0;\n      var error = void 0;\n      var didError = false;\n      try {\n        _then = entry.then;\n      } catch (e) {\n        didError = true;\n        error = e;\n      }\n\n      if (_then === then && entry._state !== PENDING) {\n        this._settledAt(entry._state, i, entry._result);\n      } else if (typeof _then !== 'function') {\n        this._remaining--;\n        this._result[i] = entry;\n      } else if (c === Promise$1) {\n        var promise = new c(noop);\n        if (didError) {\n          reject(promise, error);\n        } else {\n          handleMaybeThenable(promise, entry, _then);\n        }\n        this._willSettleAt(promise, i);\n      } else {\n        this._willSettleAt(new c(function (resolve$$1) {\n          return resolve$$1(entry);\n        }), i);\n      }\n    } else {\n      this._willSettleAt(resolve$$1(entry), i);\n    }\n  };\n\n  Enumerator.prototype._settledAt = function _settledAt(state, i, value) {\n    var promise = this.promise;\n\n\n    if (promise._state === PENDING) {\n      this._remaining--;\n\n      if (state === REJECTED) {\n        reject(promise, value);\n      } else {\n        this._result[i] = value;\n      }\n    }\n\n    if (this._remaining === 0) {\n      fulfill(promise, this._result);\n    }\n  };\n\n  Enumerator.prototype._willSettleAt = function _willSettleAt(promise, i) {\n    var enumerator = this;\n\n    subscribe(promise, undefined, function (value) {\n      return enumerator._settledAt(FULFILLED, i, value);\n    }, function (reason) {\n      return enumerator._settledAt(REJECTED, i, reason);\n    });\n  };\n\n  return Enumerator;\n}();\n\n/**\n  `Promise.all` accepts an array of promises, and returns a new promise which\n  is fulfilled with an array of fulfillment values for the passed promises, or\n  rejected with the reason of the first passed promise to be rejected. It casts all\n  elements of the passed iterable to promises as it runs this algorithm.\n\n  Example:\n\n  ```javascript\n  let promise1 = resolve(1);\n  let promise2 = resolve(2);\n  let promise3 = resolve(3);\n  let promises = [ promise1, promise2, promise3 ];\n\n  Promise.all(promises).then(function(array){\n    // The array here would be [ 1, 2, 3 ];\n  });\n  ```\n\n  If any of the `promises` given to `all` are rejected, the first promise\n  that is rejected will be given as an argument to the returned promises's\n  rejection handler. For example:\n\n  Example:\n\n  ```javascript\n  let promise1 = resolve(1);\n  let promise2 = reject(new Error(\"2\"));\n  let promise3 = reject(new Error(\"3\"));\n  let promises = [ promise1, promise2, promise3 ];\n\n  Promise.all(promises).then(function(array){\n    // Code here never runs because there are rejected promises!\n  }, function(error) {\n    // error.message === \"2\"\n  });\n  ```\n\n  @method all\n  @static\n  @param {Array} entries array of promises\n  @param {String} label optional string for labeling the promise.\n  Useful for tooling.\n  @return {Promise} promise that is fulfilled when all `promises` have been\n  fulfilled, or rejected if any of them become rejected.\n  @static\n*/\nfunction all(entries) {\n  return new Enumerator(this, entries).promise;\n}\n\n/**\n  `Promise.race` returns a new promise which is settled in the same way as the\n  first passed promise to settle.\n\n  Example:\n\n  ```javascript\n  let promise1 = new Promise(function(resolve, reject){\n    setTimeout(function(){\n      resolve('promise 1');\n    }, 200);\n  });\n\n  let promise2 = new Promise(function(resolve, reject){\n    setTimeout(function(){\n      resolve('promise 2');\n    }, 100);\n  });\n\n  Promise.race([promise1, promise2]).then(function(result){\n    // result === 'promise 2' because it was resolved before promise1\n    // was resolved.\n  });\n  ```\n\n  `Promise.race` is deterministic in that only the state of the first\n  settled promise matters. For example, even if other promises given to the\n  `promises` array argument are resolved, but the first settled promise has\n  become rejected before the other promises became fulfilled, the returned\n  promise will become rejected:\n\n  ```javascript\n  let promise1 = new Promise(function(resolve, reject){\n    setTimeout(function(){\n      resolve('promise 1');\n    }, 200);\n  });\n\n  let promise2 = new Promise(function(resolve, reject){\n    setTimeout(function(){\n      reject(new Error('promise 2'));\n    }, 100);\n  });\n\n  Promise.race([promise1, promise2]).then(function(result){\n    // Code here never runs\n  }, function(reason){\n    // reason.message === 'promise 2' because promise 2 became rejected before\n    // promise 1 became fulfilled\n  });\n  ```\n\n  An example real-world use case is implementing timeouts:\n\n  ```javascript\n  Promise.race([ajax('foo.json'), timeout(5000)])\n  ```\n\n  @method race\n  @static\n  @param {Array} promises array of promises to observe\n  Useful for tooling.\n  @return {Promise} a promise which settles in the same way as the first passed\n  promise to settle.\n*/\nfunction race(entries) {\n  /*jshint validthis:true */\n  var Constructor = this;\n\n  if (!isArray(entries)) {\n    return new Constructor(function (_, reject) {\n      return reject(new TypeError('You must pass an array to race.'));\n    });\n  } else {\n    return new Constructor(function (resolve, reject) {\n      var length = entries.length;\n      for (var i = 0; i < length; i++) {\n        Constructor.resolve(entries[i]).then(resolve, reject);\n      }\n    });\n  }\n}\n\n/**\n  `Promise.reject` returns a promise rejected with the passed `reason`.\n  It is shorthand for the following:\n\n  ```javascript\n  let promise = new Promise(function(resolve, reject){\n    reject(new Error('WHOOPS'));\n  });\n\n  promise.then(function(value){\n    // Code here doesn't run because the promise is rejected!\n  }, function(reason){\n    // reason.message === 'WHOOPS'\n  });\n  ```\n\n  Instead of writing the above, your code now simply becomes the following:\n\n  ```javascript\n  let promise = Promise.reject(new Error('WHOOPS'));\n\n  promise.then(function(value){\n    // Code here doesn't run because the promise is rejected!\n  }, function(reason){\n    // reason.message === 'WHOOPS'\n  });\n  ```\n\n  @method reject\n  @static\n  @param {Any} reason value that the returned promise will be rejected with.\n  Useful for tooling.\n  @return {Promise} a promise rejected with the given `reason`.\n*/\nfunction reject$1(reason) {\n  /*jshint validthis:true */\n  var Constructor = this;\n  var promise = new Constructor(noop);\n  reject(promise, reason);\n  return promise;\n}\n\nfunction needsResolver() {\n  throw new TypeError('You must pass a resolver function as the first argument to the promise constructor');\n}\n\nfunction needsNew() {\n  throw new TypeError(\"Failed to construct 'Promise': Please use the 'new' operator, this object constructor cannot be called as a function.\");\n}\n\n/**\n  Promise objects represent the eventual result of an asynchronous operation. The\n  primary way of interacting with a promise is through its `then` method, which\n  registers callbacks to receive either a promise's eventual value or the reason\n  why the promise cannot be fulfilled.\n\n  Terminology\n  -----------\n\n  - `promise` is an object or function with a `then` method whose behavior conforms to this specification.\n  - `thenable` is an object or function that defines a `then` method.\n  - `value` is any legal JavaScript value (including undefined, a thenable, or a promise).\n  - `exception` is a value that is thrown using the throw statement.\n  - `reason` is a value that indicates why a promise was rejected.\n  - `settled` the final resting state of a promise, fulfilled or rejected.\n\n  A promise can be in one of three states: pending, fulfilled, or rejected.\n\n  Promises that are fulfilled have a fulfillment value and are in the fulfilled\n  state.  Promises that are rejected have a rejection reason and are in the\n  rejected state.  A fulfillment value is never a thenable.\n\n  Promises can also be said to *resolve* a value.  If this value is also a\n  promise, then the original promise's settled state will match the value's\n  settled state.  So a promise that *resolves* a promise that rejects will\n  itself reject, and a promise that *resolves* a promise that fulfills will\n  itself fulfill.\n\n\n  Basic Usage:\n  ------------\n\n  ```js\n  let promise = new Promise(function(resolve, reject) {\n    // on success\n    resolve(value);\n\n    // on failure\n    reject(reason);\n  });\n\n  promise.then(function(value) {\n    // on fulfillment\n  }, function(reason) {\n    // on rejection\n  });\n  ```\n\n  Advanced Usage:\n  ---------------\n\n  Promises shine when abstracting away asynchronous interactions such as\n  `XMLHttpRequest`s.\n\n  ```js\n  function getJSON(url) {\n    return new Promise(function(resolve, reject){\n      let xhr = new XMLHttpRequest();\n\n      xhr.open('GET', url);\n      xhr.onreadystatechange = handler;\n      xhr.responseType = 'json';\n      xhr.setRequestHeader('Accept', 'application/json');\n      xhr.send();\n\n      function handler() {\n        if (this.readyState === this.DONE) {\n          if (this.status === 200) {\n            resolve(this.response);\n          } else {\n            reject(new Error('getJSON: `' + url + '` failed with status: [' + this.status + ']'));\n          }\n        }\n      };\n    });\n  }\n\n  getJSON('/posts.json').then(function(json) {\n    // on fulfillment\n  }, function(reason) {\n    // on rejection\n  });\n  ```\n\n  Unlike callbacks, promises are great composable primitives.\n\n  ```js\n  Promise.all([\n    getJSON('/posts'),\n    getJSON('/comments')\n  ]).then(function(values){\n    values[0] // => postsJSON\n    values[1] // => commentsJSON\n\n    return values;\n  });\n  ```\n\n  @class Promise\n  @param {Function} resolver\n  Useful for tooling.\n  @constructor\n*/\n\nvar Promise$1 = function () {\n  function Promise(resolver) {\n    this[PROMISE_ID] = nextId();\n    this._result = this._state = undefined;\n    this._subscribers = [];\n\n    if (noop !== resolver) {\n      typeof resolver !== 'function' && needsResolver();\n      this instanceof Promise ? initializePromise(this, resolver) : needsNew();\n    }\n  }\n\n  /**\n  The primary way of interacting with a promise is through its `then` method,\n  which registers callbacks to receive either a promise's eventual value or the\n  reason why the promise cannot be fulfilled.\n   ```js\n  findUser().then(function(user){\n    // user is available\n  }, function(reason){\n    // user is unavailable, and you are given the reason why\n  });\n  ```\n   Chaining\n  --------\n   The return value of `then` is itself a promise.  This second, 'downstream'\n  promise is resolved with the return value of the first promise's fulfillment\n  or rejection handler, or rejected if the handler throws an exception.\n   ```js\n  findUser().then(function (user) {\n    return user.name;\n  }, function (reason) {\n    return 'default name';\n  }).then(function (userName) {\n    // If `findUser` fulfilled, `userName` will be the user's name, otherwise it\n    // will be `'default name'`\n  });\n   findUser().then(function (user) {\n    throw new Error('Found user, but still unhappy');\n  }, function (reason) {\n    throw new Error('`findUser` rejected and we're unhappy');\n  }).then(function (value) {\n    // never reached\n  }, function (reason) {\n    // if `findUser` fulfilled, `reason` will be 'Found user, but still unhappy'.\n    // If `findUser` rejected, `reason` will be '`findUser` rejected and we're unhappy'.\n  });\n  ```\n  If the downstream promise does not specify a rejection handler, rejection reasons will be propagated further downstream.\n   ```js\n  findUser().then(function (user) {\n    throw new PedagogicalException('Upstream error');\n  }).then(function (value) {\n    // never reached\n  }).then(function (value) {\n    // never reached\n  }, function (reason) {\n    // The `PedgagocialException` is propagated all the way down to here\n  });\n  ```\n   Assimilation\n  ------------\n   Sometimes the value you want to propagate to a downstream promise can only be\n  retrieved asynchronously. This can be achieved by returning a promise in the\n  fulfillment or rejection handler. The downstream promise will then be pending\n  until the returned promise is settled. This is called *assimilation*.\n   ```js\n  findUser().then(function (user) {\n    return findCommentsByAuthor(user);\n  }).then(function (comments) {\n    // The user's comments are now available\n  });\n  ```\n   If the assimliated promise rejects, then the downstream promise will also reject.\n   ```js\n  findUser().then(function (user) {\n    return findCommentsByAuthor(user);\n  }).then(function (comments) {\n    // If `findCommentsByAuthor` fulfills, we'll have the value here\n  }, function (reason) {\n    // If `findCommentsByAuthor` rejects, we'll have the reason here\n  });\n  ```\n   Simple Example\n  --------------\n   Synchronous Example\n   ```javascript\n  let result;\n   try {\n    result = findResult();\n    // success\n  } catch(reason) {\n    // failure\n  }\n  ```\n   Errback Example\n   ```js\n  findResult(function(result, err){\n    if (err) {\n      // failure\n    } else {\n      // success\n    }\n  });\n  ```\n   Promise Example;\n   ```javascript\n  findResult().then(function(result){\n    // success\n  }, function(reason){\n    // failure\n  });\n  ```\n   Advanced Example\n  --------------\n   Synchronous Example\n   ```javascript\n  let author, books;\n   try {\n    author = findAuthor();\n    books  = findBooksByAuthor(author);\n    // success\n  } catch(reason) {\n    // failure\n  }\n  ```\n   Errback Example\n   ```js\n   function foundBooks(books) {\n   }\n   function failure(reason) {\n   }\n   findAuthor(function(author, err){\n    if (err) {\n      failure(err);\n      // failure\n    } else {\n      try {\n        findBoooksByAuthor(author, function(books, err) {\n          if (err) {\n            failure(err);\n          } else {\n            try {\n              foundBooks(books);\n            } catch(reason) {\n              failure(reason);\n            }\n          }\n        });\n      } catch(error) {\n        failure(err);\n      }\n      // success\n    }\n  });\n  ```\n   Promise Example;\n   ```javascript\n  findAuthor().\n    then(findBooksByAuthor).\n    then(function(books){\n      // found books\n  }).catch(function(reason){\n    // something went wrong\n  });\n  ```\n   @method then\n  @param {Function} onFulfilled\n  @param {Function} onRejected\n  Useful for tooling.\n  @return {Promise}\n  */\n\n  /**\n  `catch` is simply sugar for `then(undefined, onRejection)` which makes it the same\n  as the catch block of a try/catch statement.\n  ```js\n  function findAuthor(){\n  throw new Error('couldn't find that author');\n  }\n  // synchronous\n  try {\n  findAuthor();\n  } catch(reason) {\n  // something went wrong\n  }\n  // async with promises\n  findAuthor().catch(function(reason){\n  // something went wrong\n  });\n  ```\n  @method catch\n  @param {Function} onRejection\n  Useful for tooling.\n  @return {Promise}\n  */\n\n\n  Promise.prototype.catch = function _catch(onRejection) {\n    return this.then(null, onRejection);\n  };\n\n  /**\n    `finally` will be invoked regardless of the promise's fate just as native\n    try/catch/finally behaves\n  \n    Synchronous example:\n  \n    ```js\n    findAuthor() {\n      if (Math.random() > 0.5) {\n        throw new Error();\n      }\n      return new Author();\n    }\n  \n    try {\n      return findAuthor(); // succeed or fail\n    } catch(error) {\n      return findOtherAuther();\n    } finally {\n      // always runs\n      // doesn't affect the return value\n    }\n    ```\n  \n    Asynchronous example:\n  \n    ```js\n    findAuthor().catch(function(reason){\n      return findOtherAuther();\n    }).finally(function(){\n      // author was either found, or not\n    });\n    ```\n  \n    @method finally\n    @param {Function} callback\n    @return {Promise}\n  */\n\n\n  Promise.prototype.finally = function _finally(callback) {\n    var promise = this;\n    var constructor = promise.constructor;\n\n    if (isFunction(callback)) {\n      return promise.then(function (value) {\n        return constructor.resolve(callback()).then(function () {\n          return value;\n        });\n      }, function (reason) {\n        return constructor.resolve(callback()).then(function () {\n          throw reason;\n        });\n      });\n    }\n\n    return promise.then(callback, callback);\n  };\n\n  return Promise;\n}();\n\nPromise$1.prototype.then = then;\nPromise$1.all = all;\nPromise$1.race = race;\nPromise$1.resolve = resolve$1;\nPromise$1.reject = reject$1;\nPromise$1._setScheduler = setScheduler;\nPromise$1._setAsap = setAsap;\nPromise$1._asap = asap;\n\n/*global self*/\nfunction polyfill() {\n  var local = void 0;\n\n  if (typeof __webpack_require__.g !== 'undefined') {\n    local = __webpack_require__.g;\n  } else if (typeof self !== 'undefined') {\n    local = self;\n  } else {\n    try {\n      local = Function('return this')();\n    } catch (e) {\n      throw new Error('polyfill failed because global object is unavailable in this environment');\n    }\n  }\n\n  var P = local.Promise;\n\n  if (P) {\n    var promiseToString = null;\n    try {\n      promiseToString = Object.prototype.toString.call(P.resolve());\n    } catch (e) {\n      // silently ignored\n    }\n\n    if (promiseToString === '[object Promise]' && !P.cast) {\n      return;\n    }\n  }\n\n  local.Promise = Promise$1;\n}\n\n// Strange compat..\nPromise$1.polyfill = polyfill;\nPromise$1.Promise = Promise$1;\n\nreturn Promise$1;\n\n})));\n\n\n\n//# sourceMappingURL=es6-promise.map\n\n\n//# sourceURL=webpack://stainedglass.web/./node_modules/es6-promise/dist/es6-promise.js?");

/***/ }),

/***/ "./src/admin/app.ts":
/*!**************************!*\
  !*** ./src/admin/app.ts ***!
  \**************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {

"use strict";
eval("\nvar __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {\n    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }\n    return new (P || (P = Promise))(function (resolve, reject) {\n        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }\n        function rejected(value) { try { step(generator[\"throw\"](value)); } catch (e) { reject(e); } }\n        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }\n        step((generator = generator.apply(thisArg, _arguments || [])).next());\n    });\n};\nvar __generator = (this && this.__generator) || function (thisArg, body) {\n    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g = Object.create((typeof Iterator === \"function\" ? Iterator : Object).prototype);\n    return g.next = verb(0), g[\"throw\"] = verb(1), g[\"return\"] = verb(2), typeof Symbol === \"function\" && (g[Symbol.iterator] = function() { return this; }), g;\n    function verb(n) { return function (v) { return step([n, v]); }; }\n    function step(op) {\n        if (f) throw new TypeError(\"Generator is already executing.\");\n        while (g && (g = 0, op[0] && (_ = 0)), _) try {\n            if (f = 1, y && (t = op[0] & 2 ? y[\"return\"] : op[0] ? y[\"throw\"] || ((t = y[\"return\"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;\n            if (y = 0, t) op = [op[0] & 2, t.value];\n            switch (op[0]) {\n                case 0: case 1: t = op; break;\n                case 4: _.label++; return { value: op[1], done: false };\n                case 5: _.label++; y = op[1]; op = [0]; continue;\n                case 7: op = _.ops.pop(); _.trys.pop(); continue;\n                default:\n                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }\n                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }\n                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }\n                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }\n                    if (t[2]) _.ops.pop();\n                    _.trys.pop(); continue;\n            }\n            op = body.call(thisArg, _);\n        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }\n        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };\n    }\n};\nObject.defineProperty(exports, \"__esModule\", ({ value: true }));\n__webpack_require__(/*! es6-promise/auto */ \"./node_modules/es6-promise/auto.js\");\ndocument.querySelectorAll('.deleteChurch').forEach(function (button) {\n    var slug = button.getAttribute('data-slug');\n    // @ts-ignore\n    button.addEventListener('click', function () { return __awaiter(void 0, void 0, void 0, function () {\n        var response;\n        return __generator(this, function (_a) {\n            switch (_a.label) {\n                case 0: return [4 /*yield*/, fetch('/admin/church/' + slug, {\n                        method: 'DELETE',\n                        headers: { 'Content-Type': 'application/json' }\n                    })];\n                case 1:\n                    response = _a.sent();\n                    if (response.ok) {\n                        location.reload();\n                    }\n                    else {\n                        console.error('Delete request failed');\n                    }\n                    return [2 /*return*/];\n            }\n        });\n    }); });\n});\n\n\n//# sourceURL=webpack://stainedglass.web/./src/admin/app.ts?");

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/global */
/******/ 	(() => {
/******/ 		__webpack_require__.g = (function() {
/******/ 			if (typeof globalThis === 'object') return globalThis;
/******/ 			try {
/******/ 				return this || new Function('return this')();
/******/ 			} catch (e) {
/******/ 				if (typeof window === 'object') return window;
/******/ 			}
/******/ 		})();
/******/ 	})();
/******/ 	
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module is referenced by other modules so it can't be inlined
/******/ 	var __webpack_exports__ = __webpack_require__("./src/admin/app.ts");
/******/ 	
/******/ })()
;