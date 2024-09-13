document.addEventListener('DOMContentLoaded', function () {
    // Handle AddProject Modal
    const addProjectModal = document.getElementById('AddProject');
    const projectInput = document.getElementById('myInput'); // Ensure this id matches the input field in AddProject modal

    if (addProjectModal) {
        addProjectModal.addEventListener('shown.bs.modal', function () {
            if (projectInput) {
                projectInput.focus();
            }
        });
    }

    // Handle addTicket Modal
    const addTicketModal = document.getElementById('addTicket');
    const ticketInput = document.getElementById('ticketInput'); // Ensure this id matches the input field in addTicket modal

    if (addTicketModal) {
        addTicketModal.addEventListener('shown.bs.modal', function () {
            if (ticketInput) {
                ticketInput.focus();
            }
        });
    }
});