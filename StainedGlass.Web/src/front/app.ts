console.log("Hello, TypeScript!");
document.addEventListener('htmx:afterRequest', function(event) {
    // Your JavaScript function to be called after the AJAX request
    console.log('done!');
});