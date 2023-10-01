function statistics() {
    $("#statistics_btn").on("click",
        function(e) {
            e.preventDefault();
            e.stopPropagation();

            if ($("#statistics_box").hasClass("d-none")) {
                $.get("https://localhost:7208/api/statistics",
                    function(data) {
                        $("#total_projects").text(data.totalProjects + " projects");
                        $("#total_active_projects").text(data.totalActiveProjects + " active projects");
                        $("#total_clients").text(data.totalClients + " clients");

                        $("#statistics_box").removeClass("d-none");
                        $("#statistics_btn").text("Hide statistics");
                        $("#statistics_btn").removeClass("buttonStatistic");
                        $("#statistics_btn").addClass("buttonStatistic");
                    });
            } else {
                $("#statistics_box").addClass("d-none");
                $("#statistics_btn").text("Show statistics");
                $("#statistics_btn").removeClass("buttonStatistic");
                $("#statistics_btn").addClass("buttonStatistic");
            }
        }
    );
}

