import 'es6-promise/auto';
import SlimSelect from 'slim-select';

new SlimSelect({
    select: '#parentSlug',
})

//handle deleting
document.querySelectorAll('.delete').forEach( button => {
    var slug = button.getAttribute('data-slug');
    var type = button.getAttribute('data-type');
    // @ts-ignore
    button.addEventListener('click', async (): Promise<void> => {
        let userConfirmed = confirm("Are you sure you want to delete this item?");
        if (userConfirmed) {
            const response = await fetch('/admin/' + type + '/' + slug, {
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
        }
    });
});

document.addEventListener('DOMContentLoaded', () => {
    //handle editing
    const editForm = document.getElementById('editForm') as HTMLFormElement;
    if (editForm != null)
    {
        const slug = editForm.getAttribute('data-slug');
        const type = editForm.getAttribute('data-type');
        const redirectUrl = editForm.getAttribute('data-redirect-url') as string;
        editForm.addEventListener('submit', async (event): Promise<void> => {
            event.preventDefault();
            const formData = new FormData(editForm);
            console.log(type);
            const response = await fetch('/' + type + '/' + slug, {
                method: 'PUT',
                body: formData
            });
            if (response.ok)
            {
                location.href = redirectUrl;
            }
            else
            {
                console.error('Error submitting edit form');
            }
        });
    }
    
    //select a parent item in a select
    const addForm = document.getElementById('addForm') as HTMLFormElement;
    if (addForm != null)
    {
        addForm.addEventListener('submit', (event)  => {
            const parentSlug = document.getElementById('parentSlug') as HTMLSelectElement;
            var type = parentSlug.getAttribute('data-type');
            if (parentSlug.value == "0")
            {
                event.preventDefault();
                alert("You must select an item in the " + type + " select");
            }
        });
    }
});

async function convertFileToBase64(file: File): Promise<string> 
{ 
    return new Promise((resolve, reject) => 
    { 
        const reader = new FileReader(); 
        reader.readAsDataURL(file); 
        reader.onload = () => resolve(reader.result as string); 
        reader.onerror = error => reject(error); 
    }); 
}

