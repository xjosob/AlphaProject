/* global Quill */
document.addEventListener("DOMContentLoaded", () => {
    initQuills();
    initModals();
    initDropdowns();
    initFileUploads();
    initCustomSelects();
    initFilters();
    initFormValidation();
});

let addEditor, editEditor;

function initQuills() {
    const toolbar = [
        ["bold", "italic", "underline"],
        [{ list: "bullet" }, { list: "ordered" }],
        ["link"],
    ];

    addEditor = new Quill("#add-project-editor", {
        modules: { toolbar },
        theme: "snow",
        placeholder: "Enter project description…",
    });
    const addTa = document.getElementById("add-project-textarea");
    addEditor.on("text-change", () => {
        addTa.value = addEditor.root.innerHTML;
    });

    editEditor = new Quill("#edit-project-editor", {
        modules: { toolbar },
        theme: "snow",
        placeholder: "Enter project description…",
    });
    const editTa = document.getElementById("edit-project-textarea");
    editEditor.on("text-change", () => {
        editTa.value = editEditor.root.innerHTML;
    });
}

function initModals() {
    document.querySelectorAll("[data-type='modal']").forEach((trigger) => {
        const target = document.querySelector(trigger.dataset.target);
        trigger.addEventListener("click", () => {
            if (trigger.dataset.target === "#edit-project-modal") {
                const d = trigger.dataset;
                target.querySelector("input[name='Id']").value = d.id || "";
                target.querySelector("input[name='ProjectName']").value =
                    d.projectName || "";
                editEditor.root.innerHTML = d.description || "";
                target.querySelector("input[name='StartDate']").value =
                    d.startDate || "";
                target.querySelector("input[name='EndDate']").value =
                    d.endDate || "";
                target.querySelector("input[name='Budget']").value =
                    d.budget || "";
                target.querySelector("input[name='ClientId']").value =
                    d.clientId || "";
                target.querySelector("input[name='StatusId']").value =
                    d.statusId || "";

                const clientSelectText =
                    target.querySelector(".form-select-text");
                if (clientSelectText && d.clientId) {
                    const option = document.querySelector(
                        `.form-select-option[data-value="${d.clientId}"]`
                    );
                    if (option)
                        clientSelectText.textContent =
                            option.textContent.trim();
                }
            }
            target.classList.add("modal-show");

            const form = target.querySelector("form");
            if (form && $(form).data("validator")) {
                $(form).validate().resetForm();
                $(form)
                    .find("input, textarea, select")
                    .removeClass("input-error");
            }
        });
    });

    document.querySelectorAll("[data-type='close']").forEach((btn) => {
        const target = document.querySelector(btn.getAttribute("data-target"));
        btn.addEventListener("click", () => {
            target?.classList.remove("modal-show");

            target
                .querySelectorAll("input, textarea, select")
                .forEach((input) => {
                    if (input.type !== "hidden") {
                        input.value = "";
                        input.classList.remove("input-error");
                    }
                });

            target
                .querySelectorAll(
                    "input[name='ClientId'], input[name='StatusId']"
                )
                .forEach((input) => {
                    input.value = "";
                });

            if (target.id === "add-project-modal") {
                addEditor.root.innerHTML = "";
            } else if (target.id === "edit-project-modal") {
                editEditor.root.innerHTML = "";
            }

            target.querySelectorAll(".form-select-text").forEach((span) => {
                span.textContent =
                    span.closest(".form-select").dataset.placeholder ||
                    "Select...";
            });

            target
                .querySelectorAll("[data-file-upload]")
                .forEach((container) => {
                    const preview = container.querySelector("img");
                    const iconContainer = container.querySelector(".circle");
                    const icon = iconContainer?.querySelector("i");
                    if (preview) {
                        preview.src = "";
                        preview.classList.add("hide");
                    }
                    if (iconContainer && icon) {
                        iconContainer.classList.remove("selected");
                        icon.classList.replace("fa-pen-to-square", "fa-camera");
                    }
                });

            const form = target.querySelector("form");
            if (form && $(form).data("validator")) {
                $(form).validate().resetForm();
            }
            target.querySelectorAll(".field-error").forEach((error) => {
                error.textContent = "";
            });
        });
    });

    document.querySelectorAll("#edit-project-modal form").forEach((form) => {
        form.addEventListener("submit", async function (e) {
            e.preventDefault();
            if (!$(form).valid()) return;

            const token = form.querySelector(
                "input[name='__RequestVerificationToken']"
            ).value;

            const formData = new FormData(form);

            const response = await fetch(form.action, {
                method: "POST",
                credentials: "same-origin",
                RequestVerificationToken: token,
                body: formData,
            });

            if (response.ok) {
                window.location.reload();
            } else {
                await response.text();
            }
        });
    });
}

function initDropdowns() {
    document.addEventListener("click", (e) => {
        let clickedInsideDropdown = false;

        document
            .querySelectorAll("[data-type='dropdown']")
            .forEach((dropdownTrigger) => {
                const targetId = dropdownTrigger.getAttribute("data-target");
                const dropdown = document.querySelector(targetId);

                if (dropdownTrigger.contains(e.target)) {
                    clickedInsideDropdown = true;
                    document
                        .querySelectorAll(".dropdown.dropdown-show")
                        .forEach((open) => {
                            if (open !== dropdown)
                                open.classList.remove("dropdown-show");
                        });
                    dropdown?.classList.toggle("dropdown-show");
                }
            });

        if (!clickedInsideDropdown && !e.target.closest(".dropdown")) {
            document
                .querySelectorAll(".dropdown.dropdown-show")
                .forEach((open) => {
                    open.classList.remove("dropdown-show");
                });
        }
    });
}

// The following methods were primarily written by ChatGPT

/**
 * Initializes file upload containers so that clicking on the visual container
 * Triggers the hidden file input. Selected images are previewed directly.
 */
function initFileUploads() {
    document.querySelectorAll("[data-file-upload]").forEach((container) => {
        const input = container.querySelector("input[type='file']");
        const preview = container.querySelector("img");
        const iconContainer = container.querySelector(".circle");
        const icon = iconContainer?.querySelector("i");

        container.addEventListener("click", () => {
            input?.click();
        });

        input?.addEventListener("change", (e) => {
            const file = e.target.files[0];
            if (file && file.type.startsWith("image/")) {
                const reader = new FileReader();
                reader.onload = () => {
                    preview.src = reader.result;
                    preview.classList.remove("hide");
                    iconContainer.classList.add("selected");
                    icon.classList.replace("fa-camera", "fa-pen-to-square");
                };
                reader.readAsDataURL(file);
            }
        });
    });
}

/**
 * Initializes all custom dropdown (select) elements
 * Allows selection of clients or statuses and updating hidden input values.
 */
function initCustomSelects() {
    document.querySelectorAll(".form-select").forEach((select) => {
        const trigger = select.querySelector(".form-select-trigger");
        const triggerText = select.querySelector(".form-select-text");
        const options = select.querySelectorAll(".form-select-option");
        const hiddenInput = select.querySelector("input[type='hidden']");

        trigger?.addEventListener("click", (e) => {
            e.stopPropagation();
            document.querySelectorAll(".form-select.open").forEach((el) => {
                if (el !== select) el.classList.remove("open");
            });
            select.classList.toggle("open");
        });

        options.forEach((option) => {
            option.addEventListener("click", (e) => {
                e.stopPropagation();
                const value = option.getAttribute("data-value");

                if (
                    hiddenInput &&
                    (hiddenInput.name === "ClientId" ||
                        hiddenInput.name === "StatusId")
                ) {
                    hiddenInput.value = value;
                    triggerText.textContent = option.textContent.trim();
                    select.classList.remove("open");

                    // Trigger jQuery validation update
                    $(hiddenInput).valid();
                }
            });
        });

        document.addEventListener("click", (e) => {
            if (!select.contains(e.target)) select.classList.remove("open");
        });

        document.querySelectorAll("[data-type='close']").forEach((btn) => {
            btn.addEventListener("click", () => {});
        });
    });
}

/**
 * Initializes filter buttons that show/hide project cards based on their status (all, started, completed).
 */
function initFilters() {
    document
        .querySelectorAll(".project-filters .filter-button")
        .forEach((btn) => {
            btn.addEventListener("click", () => {
                const filter = btn.getAttribute("data-filter");
                document.querySelectorAll(".project.card").forEach((card) => {
                    const status = card.getAttribute("data-status");
                    card.style.display =
                        filter === "all" || status === filter
                            ? "block"
                            : "none";
                });
                document
                    .querySelectorAll(".project-filters .filter-button")
                    .forEach((b) => b.classList.remove("active"));
                btn.classList.add("active");
            });
        });
}

/**
 * Sets up jQuery validation for all forms on the page
 * Defines custom rules, messages, and error placement.
 */
function initFormValidation() {
    $.validator.setDefaults({
        errorElement: "span",
        errorClass: "field-error",
        errorPlacement: function (error, element) {
            if (element.closest(".form-select").length) {
                error.appendTo(element.closest(".form-select"));
            } else if (element.parent(".form-input-extended").length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        },
        highlight: function (element) {
            $(element).addClass("input-error");
        },
        unhighlight: function (element) {
            $(element).removeClass("input-error");
        },
    });

    $("form").each(function () {
        const form = $(this);

        form.validate({
            ignore: [],
            rules: {
                ProjectName: {
                    required: true,
                    minlength: 2,
                },
                StartDate: {
                    required: true,
                    date: true,
                },
                EndDate: {
                    date: true,
                },
                ClientId: {
                    required: true,
                },
                StatusId: {
                    required: true,
                },
            },
            messages: {
                ProjectName: {
                    required: "Please enter a project name.",
                },
                StartDate: {
                    required: "Please choose a start date.",
                },
                ClientId: {
                    required: "Please select a client.",
                },
                StatusId: {
                    required: "Please select a status.",
                },
            },
        });

        form.on("submit", function (e) {
            if (!form.valid()) {
                e.preventDefault();
                return false;
            }
        });
    });
}
