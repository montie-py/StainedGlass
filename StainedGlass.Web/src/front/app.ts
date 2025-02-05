const overlayElement = document.getElementById('overlay') as HTMLDivElement;

//event, called after an htmx AJAX request
document.addEventListener('htmx:afterRequest', function(event) {
    
    //activate the overlay element
    if (overlayElement)
    {
        overlayElement.style.display = 'block';
    }
    
    //hide the previous popups' content, after the last content is loaded due to the AJAX request
    const popupContentDivs = document.querySelectorAll('.popup-content') as unknown as HTMLDivElement[];
    popupContentDivs.forEach((div, index) => {
        if (index < popupContentDivs.length - 1)
        {
            div.style.display = 'none';
        }
    });
    
    const popupFunctionality = new PopupFunctionality();
    popupFunctionality.closePopup();
    popupFunctionality.goBack();
});

class PopupFunctionality {
    goBack() {
        const backButtons = document.getElementsByClassName('back') as unknown as HTMLCollection;
        if (backButtons)
        {
            const backButtonsElementsArray = Array.from(backButtons);
            
            backButtonsElementsArray.forEach(backButtonElement => {
                backButtonElement.addEventListener('click', (e) => {
                    const popupContentDivs = document.querySelectorAll('.popup-content') as unknown as HTMLDivElement[];

                    //removing the current .popup-content element and revealing the previous one 
                    popupContentDivs[popupContentDivs.length - 1].remove();
                    popupContentDivs[popupContentDivs.length - 2].style.display = 'block';
                }); 
            });
        }
    }

    closePopup() {
        const closePopupElements = document.getElementsByClassName('close-popup') as unknown as HTMLCollection;
        if (closePopupElements)
        {
            const closePopupElementsArray = Array.from(closePopupElements);
            closePopupElementsArray.forEach(closePopupElement => {
                closePopupElement.addEventListener('click', function(event) {
                    if (overlayElement)
                    {
                        overlayElement.style.display = 'none';
                    }
                    const popupContentDivs = document.querySelectorAll('.popup-content') as unknown as HTMLDivElement[];
                    popupContentDivs.forEach((div) => {
                        div.remove();
                    });
                });
            })
        }
    }   
}