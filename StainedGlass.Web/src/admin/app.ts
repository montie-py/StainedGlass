import 'es6-promise/auto';
import SlimSelect from 'slim-select';

new SlimSelect({
    select: '#parentSlug',
});

new SlimSelect({
    select: '#itemTypeSelect'
});

new SlimSelect({
    select: '#relatedItemsSelect'
});


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

function filePositioning()
{
    const parentImage = document.getElementById("parentImage") as HTMLImageElement;

    if (parentImage != null)
    {
        parentImage.addEventListener('click', (element) => {
            //removing last inserted position
            const elementPosition = document.getElementById("elementPosition") as HTMLElement;
            if (elementPosition != null)
            {
                elementPosition.remove();
            }
            
            var parentImage = element.target as HTMLImageElement;
            const parentImageHeight = parentImage.clientHeight;
            const parentImageWidth = parentImage.clientWidth;

            //creating new position
            const iElement = document.createElement('i') as HTMLElement;
            const offsetY = element.offsetY-10;
            const offsetX = element.offsetX-8;
            const offsetYPercent = (offsetY * 100 / parentImageHeight) + "%"
            const offsetXPercent = (offsetX * 100 / parentImageWidth) + "%";

            iElement.classList.add('bi', 'bi-asterisk');
            iElement.id = "elementPosition";
            iElement.style.position = 'absolute';
            iElement.style.top = offsetYPercent;
            iElement.style.left = offsetXPercent;
            parentImage.insertAdjacentElement('afterend', iElement);

            //insert position into the input
            var positionInput = document.getElementById("positionInput") as HTMLInputElement;
            positionInput.value = "left:" + offsetXPercent + "; top:" + offsetYPercent + ";";
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

document.addEventListener('DOMContentLoaded', () => {
    //add/remove item image
    // addRemoveItemImage();
    
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
    if (parentSlug)
    {
        parentSlug.addEventListener('change', (event) => {
            const parentBase64Container = document.getElementById('parentImages') as HTMLDivElement;
            const positionContainer = document.getElementById('positionContainer') as HTMLDivElement;
            const parentImage = document.getElementById("parentImage") as HTMLImageElement;
            if (parentImage != null)
            {
                positionContainer.innerHTML = '';

            }
            if (parentSlug.value !== "0")
            {
                var base64Element = parentBase64Container.querySelector('.' + parentSlug.value) as HTMLParagraphElement;
                const selectedChurchImg = document.createElement("img") as HTMLImageElement;
                selectedChurchImg.id = "parentImage";
                selectedChurchImg.src = "data:image/jpeg;base64," + base64Element.textContent;

                selectedChurchImg.style.width = "150px";
                positionContainer.insertAdjacentElement("afterbegin", selectedChurchImg);
                filePositioning();
            }
        });   
    }
    
    //for the 'edit' page
    filePositioning();
});

