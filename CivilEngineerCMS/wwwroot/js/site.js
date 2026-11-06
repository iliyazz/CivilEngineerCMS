function statistics() {
    $("#statistics_btn").on("click",
        function(e) {
            e.preventDefault();
            e.stopPropagation();

            if ($("#statistics_box").hasClass("d-none")) {
                // Same-origin endpoint in the admin area (Admin/Home/Statistics).
                $.get($("#statistics_btn").data("url"))
                    .done(function(data) {
                        // The card labels describe the numbers; button texts come from data-* attributes in the current language.
                        $("#total_projects").text(data.totalProjects);
                        $("#total_active_projects").text(data.totalActiveProjects);
                        $("#total_clients").text(data.totalClients);

                        $("#statistics_box").removeClass("d-none");
                        $("#statistics_btn").text($("#statistics_btn").data("hide-text"));
                        $("#statistics_btn").removeClass("buttonStatistic");
                        $("#statistics_btn").addClass("buttonStatistic");
                    })
                    .fail(function() {
                        toastr.error($("#statistics_btn").data("error-text"));
                    });
            } else {
                $("#statistics_box").addClass("d-none");
                $("#statistics_btn").text($("#statistics_btn").data("show-text"));
                $("#statistics_btn").removeClass("buttonStatistic");
                $("#statistics_btn").addClass("buttonStatistic");
            }
        }
    );
}