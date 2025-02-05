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

eval("\nvar overlayElement = document.getElementById('overlay');\n//event, called after an htmx AJAX request\ndocument.addEventListener('htmx:afterRequest', function (event) {\n    //activate the overlay element\n    if (overlayElement) {\n        overlayElement.style.display = 'block';\n    }\n    //hide the previous popups' content, after the last content is loaded due to the AJAX request\n    var popupContentDivs = document.querySelectorAll('.popup-content');\n    popupContentDivs.forEach(function (div, index) {\n        if (index < popupContentDivs.length - 1) {\n            div.style.display = 'none';\n        }\n    });\n    var popupFunctionality = new PopupFunctionality();\n    popupFunctionality.closePopup();\n    popupFunctionality.goBack();\n});\nvar PopupFunctionality = /** @class */ (function () {\n    function PopupFunctionality() {\n    }\n    PopupFunctionality.prototype.goBack = function () {\n        var backButtons = document.getElementsByClassName('back');\n        if (backButtons) {\n            var backButtonsElementsArray = Array.from(backButtons);\n            backButtonsElementsArray.forEach(function (backButtonElement) {\n                backButtonElement.addEventListener('click', function (e) {\n                    var popupContentDivs = document.querySelectorAll('.popup-content');\n                    //removing the current .popup-content element and revealing the previous one \n                    popupContentDivs[popupContentDivs.length - 1].remove();\n                    popupContentDivs[popupContentDivs.length - 2].style.display = 'block';\n                });\n            });\n        }\n    };\n    PopupFunctionality.prototype.closePopup = function () {\n        var closePopupElements = document.getElementsByClassName('close-popup');\n        if (closePopupElements) {\n            var closePopupElementsArray = Array.from(closePopupElements);\n            closePopupElementsArray.forEach(function (closePopupElement) {\n                closePopupElement.addEventListener('click', function (event) {\n                    if (overlayElement) {\n                        overlayElement.style.display = 'none';\n                    }\n                    var popupContentDivs = document.querySelectorAll('.popup-content');\n                    popupContentDivs.forEach(function (div) {\n                        div.remove();\n                    });\n                });\n            });\n        }\n    };\n    return PopupFunctionality;\n}());\n\n\n//# sourceURL=webpack://stainedglass.web/./src/front/app.ts?");

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