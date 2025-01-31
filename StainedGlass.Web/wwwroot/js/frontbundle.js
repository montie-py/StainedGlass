/*
 * ATTENTION: The "eval" devtool has been used (maybe by default in mode: "development").
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./src/front/app.ts":
/*!**************************!*\
  !*** ./src/front/app.ts ***!
  \**************************/
/***/ (() => {

eval("\nconsole.log(\"Hello, TypeScript!\");\nvar overlayElement = document.getElementById('overlay');\ndocument.addEventListener('htmx:afterRequest', function (event) {\n    // Your JavaScript function to be called after the AJAX request\n    if (overlayElement) {\n        overlayElement.style.display = 'block';\n    }\n    closePopupFunctionality();\n});\nfunction closePopupFunctionality() {\n    var closePopupElement = document.getElementById('closePopup');\n    if (closePopupElement) {\n        closePopupElement.addEventListener('click', function (event) {\n            if (overlayElement) {\n                overlayElement.style.display = 'none';\n            }\n            var popupContentElement = closePopupElement.closest('.popup-content');\n            if (popupContentElement) {\n                popupContentElement.remove();\n            }\n        });\n    }\n}\n\n\n//# sourceURL=webpack://stainedglass.web/./src/front/app.ts?");

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module can't be inlined because the eval devtool is used.
/******/ 	var __webpack_exports__ = {};
/******/ 	__webpack_modules__["./src/front/app.ts"]();
/******/ 	
/******/ })()
;