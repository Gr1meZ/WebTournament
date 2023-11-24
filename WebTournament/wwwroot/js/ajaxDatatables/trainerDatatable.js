﻿let GetTrainerDatatable = function () {

    let $deleteItemModal = $('#deleteItemModal');
    let $editItemModal = $('#myModal');
    let $editDialogItemModal = $('#dialogContent');


    function init() {
        const $tbl = $('#item-tbl').DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/ru.json'},
            searching: true,
            "bLengthChange": false,
            searchDelay: 300,
            stateSave: true,
            processing: true,
            serverSide: true,
            ordering: true,
            order: [[ 1, "desc" ]],
            rowId: 'id',
            ajax: function(data, callback, settings) {
                dtAjaxHandler('/Trainer/List', data, callback)
                
            },
            columnDefs: [
                { targets: 0, title: "ID", data: "id", visible: false },
                { targets: 1, title: "Клуб", data: "clubName" },
                { targets: 2, title: "Имя", data: "name" },
                { targets: 3, title: "Фамилия", data: "surname" },
                { targets: 4, title: "Отчество", data: "patronymic" },
                { targets: 5, title: "Телефон", data: "phone" },

                {
                    targets: 6, title: "", data: "id", className: "text-center", orderable: false,
                    render: function ( itemId, type, row, meta ) {

                        return `<button  class="btn btn-info" data-action="showEditItemModal" id="showEditItemModal"><i class="bi bi-pencil"></i></button>`

                    }
                },
                {
                    targets: 7, title: "", data: "itemId", className: "text-center", orderable: false,
                    render: function ( itemId, type, row, meta ) {

                        return `<button data-action="showDeleteItemModal" type="button" class="btn btn-danger " title="Delete item"><i class="bi bi-trash"></i></button>`

                    }
                }
            ]
        });

        $tbl.on('click', 'button[data-action="showDeleteItemModal"]', function (ev) {
            let $row = $(ev.currentTarget).closest('tr');
            console.log($row)
            let itemId = $row.attr('id');
            let itemName = $("#" + itemId).find("td:eq(0)").text();
            $deleteItemModal.data('itemId', itemId)
            $deleteItemModal.find('.modal-body').text(`Вы действительно хотите удалить категорию '${itemName}'?`)
            $deleteItemModal.modal('show')
        })

        $tbl.on('click', 'button[data-action="showEditItemModal"]', function (ev) {
            let $row = $(ev.currentTarget).closest('tr');
            let itemId = $row.attr('id');
            $.get(`/Trainer/${itemId}/EditIndex`, function(data){
                $editDialogItemModal.html(data);
                $editItemModal.data('itemId', itemId)
                $editItemModal.modal('show')
            });


        })

        $('#deleteItemBtn').click(function () {
            let itemId = $deleteItemModal.data('itemId')
            console.log("test")
            console.log(itemId)
            $.ajax({
                url: '/Trainer/' + itemId,
                type: 'DELETE',
                success: function () {
                    $tbl.ajax.reload();
                    $deleteItemModal.modal('hide');
                }
            })
        })
    }

    return {
        init
    }
}