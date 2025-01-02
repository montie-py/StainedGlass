import 'es6-promise/auto'; 

//handle church deleting
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
    //handle church editing
    const editForm = document.getElementById('editForm') as HTMLFormElement;
    if (editForm != null)
    {
        const churchSlug = editForm.getAttribute('data-slug');
        const redirectUrl = editForm.getAttribute('data-redirect-url') as string;
        editForm.addEventListener('submit', async (event): Promise<void> => {
            event.preventDefault();
            const formData = new FormData(editForm);

            const response = await fetch('/church/' + churchSlug, {
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
    
    //select an item in the church select (sanctuarySide page)
    const addForm = document.getElementById('addForm') as HTMLFormElement;
    if (addForm != null)
    {
        addForm.addEventListener('submit', (event)  => {
            const churchSlug = document.getElementById('churchSlug') as HTMLSelectElement;
            if (churchSlug.value == "0")
            {
                event.preventDefault();
                alert("You must select an item in the Church select");
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

