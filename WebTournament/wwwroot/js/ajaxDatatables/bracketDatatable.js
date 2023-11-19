let GetBracketDatatable = function (tournamentId) {

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
            ajax: function(data, callback) {
                dtAjaxHandler('/Bracket/List?tournamentId=' + tournamentId, data, callback)
                
            },
            columnDefs: [
                { targets: 0, title: "ID", data: "id", visible: false },
                { targets: 1, title: "Дивизион", data: "divisionName" },
                { targets: 2, title: "Категория", data: "categoriesName" },
                { targets: 3, title: "Вес, кг", data: "maxWeight" },
                
                {
                    targets: 4, title: "", data: "itemId", className: "text-center", orderable: false,
                    render: function () {

                        return `<button data-action="showDeleteItemModal" type="button" class="btn btn-danger " title="Delete item"><i class="bi bi-trash"></i></button>`

                    }
                }
            ]
        });

        $tbl.on('click', 'button[data-action="showDeleteItemModal"]', function (ev) {
            let $row = $(ev.currentTarget).closest('tr');
            console.log($row)
            let itemId = $row.attr('id');
            let itemNumber = $("#" + itemId).find("td:eq(0)").text();
            let itemShortName = $("#" + itemId).find("td:eq(1)").text();
            $deleteItemModal.data('itemId', itemId)
            $deleteItemModal.find('.modal-body').text(`Вы действительно хотите удалить пояс '${itemNumber} ${itemShortName}'?`)
            $deleteItemModal.modal('show')
        })
        
        $('#deleteItemBtn').click(function () {
            let itemId = $deleteItemModal.data('itemId')
            console.log("test")
            console.log(itemId)
            $.ajax({
                url: '/Bracket/' + itemId,
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