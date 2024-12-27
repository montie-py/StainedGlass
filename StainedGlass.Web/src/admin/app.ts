import 'es6-promise/auto'; 
document.querySelectorAll('.deleteChurch').forEach( button => {
    var slug = button.getAttribute('data-slug');
    // @ts-ignore
    button.addEventListener('click', async (): Promise<void> => {
       const response = await fetch('/admin/church/' + slug, {
           method: 'DELETE',
           headers: { 'Content-Type': 'application/json' }
       });
        if (response.ok) 
        { 
            location.reload();
            } 
        else 
        { 
            console.error('Delete request failed'); 
        }
    });
});