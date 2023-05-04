﻿//$(document).ready(function () {
//    LoadDataTable();
//});

//function LoadDataTable() {
//    dataTable = $('#tbldata').DataTable({
//        "ajax": {url: '/Admin/ProductType/GetAll'},
//        "columns": [
//            { data: 'title', "width": "10%" },
//            { data: 'isbn', "width": "10%" },
//            { data: 'author', "width": "10%" },
//            { data: 'listPrice', "width": "10%" },
//            { data: 'listPrice50', "width": "10%" },
//            { data: 'category.name', "width": "10%" }
//        ]
//    });
//}

var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/ProductType/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "listPrice", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "listPrice50", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return ` 
                            <div class="w-75  btn-group" role="group">
                            <a href="/Admin/ProductType/Upsert?id=${data} " 
                            class="btn btn-primary mx - 2">
                            <i class="bi bi-pencil-square"></i>Edit
                            </a>
                            <a onClick=Delete('/Admin/ProductType/Delete/'+${data})
                            class="btn  btn-danger  mx-2">
                            <i class="bi bi-trash"></i>Delete
                            </a>  
                            </div>
                            `
                },
                "width": "15%"
            },
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        //To delete the data from the API endpoint
        if (result.isConfirmed) {
            $.ajax(
                {
                    url: url,
                    type: "DELETE",
                    success: function (data) {
                        if (data.success) {
                            dataTable.ajax.reload();
                            toastr.success(data.message);
                        }
                        else {
                            toastr.error(data.message);
                        }

                    }
                })
        }
    })
}