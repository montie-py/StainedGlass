console.log("Hello, TypeScript!");

const overlayElement = document.getElementById('overlay') as HTMLDivElement;
document.addEventListener('htmx:afterRequest', function(event) {
    // Your JavaScript function to be called after the AJAX request
    
    if (overlayElement)
    {
        overlayElement.style.display = 'block';
    }
    
    closePopupFunctionality();
});

function closePopupFunctionality() {
    const closePopupElement = document.getElementById('closePopup') as HTMLElement;
    if (closePopupElement)
    {
        closePopupElement.addEventListener('click', function(event) {
            if (overlayElement)
            {
                overlayElement.style.display = 'none';
            }
            const popupContentElement = closePopupElement.closest('.popup-content') as HTMLElement;
            if (popupContentElement)
            {
                popupContentElement.remove();
            }
        });
    }   
}