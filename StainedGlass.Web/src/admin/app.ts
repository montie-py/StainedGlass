import 'es6-promise/auto'; 

//handle church deleting
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

document.addEventListener('DOMContentLoaded', () => {
    //handle church editing
    const editForm = document.getElementById('editForm') as HTMLFormElement;
    const churchSlug = editForm.getAttribute('data-slug');
    const redirectUrl = editForm.getAttribute('data-redirect-url') as string;
    editForm.addEventListener('submit', async (event): Promise<void> => {
        event.preventDefault();
        const formData = new FormData(editForm);
        // const data: { [key: string]: string } = {};
        // formData.forEach((value, key) => {
        //     data[key] = value as string;
        // });
        // //if an image is uploaded - convert it to base64
        // const file = formData.get('Image') as File;
        // if (file.name.length > 0)
        // {
        //     const base64File = await convertFileToBase64(file);
        //     data.Image = base64File;
        // } else
        // {
        //     //if not - do not send it to the server;
        //     delete data.Image;
        // }
        
        // console.log(data);

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

