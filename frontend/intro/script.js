const form = document.getElementById('catForm');
const catTableBody = document.querySelector('#catTable tbody');
let catId = 1;
let editingCatRow = null;
let cats = [];

loadCatsFromLocalStorage();

form.addEventListener('submit', function(e) {
    e.preventDefault();

    const catName = document.getElementById('catName').value;
    const catAge = document.getElementById('catAge').value;
    const catColor = document.getElementById('catColor').value;

    if (editingCatRow) {
        updateCat(editingCatRow, catName, catColor, catAge);
    } else {
        addCat(catName, catColor, catAge);
    }

    form.reset();
    editingCatRow = null;
});

function addCat(name, color, age) {
    const row = document.createElement('tr');
    row.innerHTML = `
        <td>${catId}</td>
        <td>${name}</td>
        <td>${color}</td>
        <td>${age}</td>
        <td><button class="edit-btn">Edit</button></td>
        <td><button class="delete-btn">Delete</button></td>
    `;
    catTableBody.appendChild(row);

    row.querySelector('.edit-btn').addEventListener('click', function() {
        editCat(row);
    });
    row.querySelector('.delete-btn').addEventListener('click', function() {
        deleteCat(row);
    });


    const cat = { id: catId++, name, color, age };
    cats.push(cat);
    saveCatsToLocalStorage();
}

function updateCat(row, name, color, age) {
    const rowId = row.children[0].textContent;
    row.children[1].textContent = name;
    row.children[2].textContent = color;
    row.children[3].textContent = age;

    const catIndex = cats.findIndex(cat => cat.id == rowId);
    cats[catIndex] = { id: rowId, name, color, age };

    saveCatsToLocalStorage();
}

function editCat(row) {
    const name = row.children[1].textContent;
    const color = row.children[2].textContent;
    const age = row.children[3].textContent;

    document.getElementById('catName').value = name;
    document.getElementById('catColor').value = color;
    document.getElementById('catAge').value = age;

    editingCatRow = row;
}

function deleteCat(row) {
    if(confirm("Do you want to delete this cat from the table?") !== true){
        return
    }
    const rowId = row.children[0].textContent;
    row.remove();
    cats = cats.filter(cat => cat.id != rowId);
    saveCatsToLocalStorage();
}

function saveCatsToLocalStorage() {
    localStorage.setItem('cats', JSON.stringify(cats));
}

function loadCatsFromLocalStorage() {
    const storedCats = localStorage.getItem('cats');
    if (storedCats) {
        cats = JSON.parse(storedCats);
        cats.forEach(cat => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${cat.id}</td>
                <td>${cat.name}</td>
                <td>${cat.color}</td>
                <td>${cat.age}</td>
                <td><button class="edit-btn">Edit</button></td>
                <td><button class="delete-btn">Delete</button></td>
            `;
            catTableBody.appendChild(row);

            row.querySelector('.edit-btn').addEventListener('click', function() {
                editCat(row);
            });
            row.querySelector('.delete-btn').addEventListener('click', function() {
                deleteCat(row);
            });
        });

        if (cats.length > 0) {
            catId = cats[cats.length - 1].id + 1;
        }
    }
}
