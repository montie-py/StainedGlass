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

    const parentSlug = document.getElementById('parentSlug') as HTMLSelectElement;
    
    //select a parent item in a select
    const addForm = document.getElementById('addForm') as HTMLFormElement;
    if (addForm != null)
    {
        addForm.addEventListener('submit', (event)  => {
            var type = parentSlug.getAttribute('data-type');
            if (parentSlug.value == "0")
            {
                event.preventDefault();
                alert("You must select an item in the " + type + " select");
            }
        });
    }
    
    //loading new image for positioning when new parent is selected
    parentSlug.addEventListener('change', (event) => {
        const parentBase64Container = document.getElementById('parentImages') as HTMLDivElement;
        const positionContainer = document.getElementById('positionContainer') as HTMLDivElement;
        const churchImage = document.getElementById("churchImage") as HTMLImageElement;
        if (churchImage != null)
        {
            positionContainer.innerHTML = '';

        }
        if (parentSlug.value !== "0")
        {
            var base64Element = parentBase64Container.querySelector('.' + parentSlug.value) as HTMLParagraphElement;
            const selectedChurchImg = document.createElement("img") as HTMLImageElement;
            selectedChurchImg.id = "churchImage";
            selectedChurchImg.src = "data:image/jpeg;base64," + base64Element.textContent;

            selectedChurchImg.style.width = "150px";
            positionContainer.insertAdjacentElement("afterbegin", selectedChurchImg);
            filePositioning();
        }
    });
    
    //for the 'edit' page
    filePositioning();
});

function filePositioning()
{
    const churchImage = document.getElementById("churchImage") as HTMLImageElement;

    if (churchImage != null)
    {
        churchImage.addEventListener('click', (element) => {
            //removing last inserted position
            const elementPosition = document.getElementById("elementPosition") as HTMLElement;
            if (elementPosition != null)
            {
                elementPosition.remove();
            }

            //creating new position
            const iElement = document.createElement('i') as HTMLElement;
            const offsetY = element.offsetY-10;
            const offsetX = element.offsetX-8;
            
            iElement.classList.add('bi', 'bi-asterisk');
            iElement.id = "elementPosition";
            iElement.style.position = 'absolute';
            iElement.style.top = offsetY + "px";
            iElement.style.left = offsetX + "px";
            churchImage.insertAdjacentElement('afterend', iElement);
            
            //insert position into the input
            var positionInput = document.getElementById("positionInput") as HTMLInputElement;
            positionInput.value = "left:" + offsetX + "px; top:" + offsetY + "px;";
        });
    }
}

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

