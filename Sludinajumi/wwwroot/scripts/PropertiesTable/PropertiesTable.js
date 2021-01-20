import { Property } from "../Models/Property";
import { Dialog } from "../Dialog/Dialog";
import { DialogButton } from "../Dialog/Buttons/DialogButton";
import { DefaultButtonStyle } from "../Dialog/Buttons/DefaultButtonStyle";
var PropertiesTable = (function () {
    function PropertiesTable() {
        var _this = this;
        this.enterValuePlaceholder = "#Nozīme!";
        this.manageTranslationsText = "Tulkot";
        this.removePropertyText = "Dzēst";
        this.properties = new Array();
        this.propertiesTable = document.querySelector("#propertiesTable");
        this.searchInputPropertiesTable = document.querySelector("#searchInputPropertiesTable");
        this.searchInputPropertiesTable.addEventListener("keyup", function (event) {
            return _this.searchPropertiesTable(_this.searchInputPropertiesTable, event);
        });
        this.randomDataSeed();
        this.updateTable();
        this.newPropertyNameField.addEventListener("focus", function (event) {
            return _this.newPropertyField_Click(_this.newPropertyNameField, event);
        });
        this.newPropertyDescriptionField.addEventListener("focus", function (event) {
            return _this.newPropertyField_Click(_this.newPropertyDescriptionField, event);
        });
        this.newPropertyNameField.addEventListener("blur", function (event) {
            return _this.newPropertyField_Blur(_this.newPropertyNameField, event);
        });
        this.newPropertyDescriptionField.addEventListener("blur", function (event) {
            return _this.newPropertyField_Blur(_this.newPropertyDescriptionField, event);
        });
        this.addProperty.addEventListener("click", function (event) { return _this.addProperty_Click(_this.addProperty, event); });
        var submitButton = document.getElementById("createNewCategory");
        submitButton.addEventListener("click", function (e) { return _this.submitButton_Click(submitButton, e); });
        var newCategoryForm = document.getElementById("newCategoryForm");
        newCategoryForm.addEventListener("submit", function (e) { return _this.submitButton_Click(_this, e); });
        var inputs = newCategoryForm.querySelectorAll("input, select, textarea");
        (inputs).forEach(function (input) {
            input.addEventListener("input", function () {
                var validationError = document.getElementById(input.name + "_ValidationMessage");
                if (validationError) {
                    validationError.classList.add("w3-hide");
                }
            });
            input.addEventListener("change", function () {
                var validationError = document.getElementById(input.name + "_ValidationMessage");
                if (validationError) {
                    validationError.classList.add("w3-hide");
                }
            });
        });
    }
    Object.defineProperty(PropertiesTable.prototype, "propertiesTableBody", {
        get: function () {
            return this.propertiesTable.tBodies[0];
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PropertiesTable.prototype, "propertiesTableRows", {
        get: function () {
            return this.propertiesTableBody.querySelectorAll("tr");
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PropertiesTable.prototype, "addProperty", {
        get: function () {
            return document.getElementById("addProperty");
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PropertiesTable.prototype, "newPropertyNameField", {
        get: function () {
            return document.getElementById("newPropertyNameField");
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PropertiesTable.prototype, "newPropertyDescriptionField", {
        get: function () {
            return document.getElementById("newPropertyDescriptionField");
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PropertiesTable.prototype, "newPropertyIsRequiredField", {
        get: function () {
            return document.getElementById("newPropertyIsRequiredField");
        },
        enumerable: true,
        configurable: true
    });
    PropertiesTable.prototype.searchPropertiesTable = function (sender, event) {
        var filter = this.searchInputPropertiesTable.value.toUpperCase();
        var cell;
        for (var i = 0; i < this.propertiesTableRows.length; i++) {
            cell = this.propertiesTableRows[i].querySelectorAll("td")[0];
            if (cell != null) {
                if (cell.innerText.toUpperCase().indexOf(filter) > -1)
                    this.propertiesTableRows[i].style.display = "";
                else
                    this.propertiesTableRows[i].style.display = "none";
            }
        }
    };
    PropertiesTable.prototype.randomDataSeed = function () {
        var colorProperty = new Property("Krāsa", "Objekta krāsa");
        colorProperty.addTranslation(2, "Color", "Object color");
        colorProperty.addTranslation(3, "Цвет", "Цвет объекта");
        colorProperty.isRequired = true;
        var sizeProperty = new Property("Izmēri", "Objekta izmēri ({X} cm x {Y} cm");
        sizeProperty.addTranslation(2, "Size", "");
        sizeProperty.addTranslation(3, "Размер", "");
        colorProperty.isRequired = false;
        this.properties.push(colorProperty);
        this.properties.push(sizeProperty);
    };
    PropertiesTable.prototype.updateValues = function () {
        var _this = this;
        this.propertiesTableRows.forEach(function (row, index) {
            var objectRef = parseInt(row.dataset.objectRef);
            var propertyNameCell = row.querySelector("td[data-object-ref-type='name']");
            var propertyDescriptionCell = row.querySelector("td[data-object-ref-type='description']");
            var propertyIsRequiredCheckBox = row.querySelector("input[data-object-ref-type='isRequired']");
            if (propertyNameCell == null || propertyDescriptionCell == null || propertyIsRequiredCheckBox == null) {
                console.log(propertyNameCell);
                console.log(propertyDescriptionCell);
                console.log(propertyIsRequiredCheckBox);
                return;
            }
            _this.properties[objectRef].Name = propertyNameCell.innerText;
            _this.properties[objectRef].Description = propertyDescriptionCell.innerText;
            _this.properties[objectRef].isRequired = propertyIsRequiredCheckBox.checked;
        });
        console.log(this.properties);
    };
    PropertiesTable.prototype.editor_Blur = function (sender, event) {
        if (sender.innerText == "") {
            this.updateValues();
            sender.innerText = this.enterValuePlaceholder;
        }
        else {
            var value = sender.innerText;
            var parentCell = sender.parentElement;
            parentCell.removeChild(sender);
            parentCell.innerText = value;
            this.updateValues();
        }
    };
    PropertiesTable.prototype.newPropertyField_Click = function (sender, event) {
        if (sender.innerText == "(Nosaukums)..." || sender.innerText == "(Apraksts)...")
            sender.innerText = "";
        sender.focus();
    };
    PropertiesTable.prototype.newPropertyField_Blur = function (sender, event) {
        if (sender.innerText == "")
            if (sender == this.newPropertyNameField)
                sender.innerText = "(Nosaukums)...";
            else if (sender == this.newPropertyDescriptionField)
                sender.innerText = "(Apraksts)...";
    };
    PropertiesTable.prototype.editor_KeyDown = function (sender, event) {
        var sd = sender;
        if (event.defaultPrevented)
            return;
        else if (event.key == "Enter")
            sender.blur();
        else if (event.key == "Backspace" && sd.selectionStart == -1)
            event.preventDefault();
    };
    PropertiesTable.prototype.tableCell_DblClick = function (sender, event) {
        var _this = this;
        var textValue = sender.innerText;
        var editor = document.createElement("span");
        if (textValue == this.enterValuePlaceholder)
            editor.innerText = "";
        else
            editor.innerText = textValue;
        editor.contentEditable = "true";
        sender.innerText = "";
        sender.appendChild(editor);
        editor.addEventListener("blur", function (event) {
            return _this.editor_Blur(editor, event);
        });
        editor.addEventListener("keydown", function (event) {
            return _this.editor_KeyDown(editor, event);
        });
        editor.focus();
    };
    PropertiesTable.prototype.manageTranslationsSpan_Click = function (property, event) {
        var _this = this;
        var index = this.properties.indexOf(property);
        if (index == -1) {
            console.error("Property wasn't found!");
            return;
        }
        var dialog = new Dialog();
        dialog.title = "Parvaldīt tulkojumus";
        dialog.description = "<p>Šeit Jūs varat tulkot īpašību, lai tā būtu pieejama arī citas valodās.</p>";
        var ok = new DialogButton("Labi", new DefaultButtonStyle(), true);
        ok.onClick = function (s, e) {
            dialog.closeDialog();
        };
        dialog.buttons = new Array(ok);
        dialog.showDialog();
        dialog.assignForm("formWorld", property);
        dialog.onFormSubmit = function (s, e) {
            _this.properties[index] = Property.fromObject(dialog.formDataToObject());
            _this.updateTable();
        };
    };
    PropertiesTable.prototype.removePropertySpan_Click = function (property, event) {
        var index = this.properties.indexOf(property);
        if (index > -1) {
            this.properties.splice(index, 1);
            console.info("Property is now removed");
            this.updateTable();
        }
    };
    PropertiesTable.prototype.updateTable = function () {
        var _this = this;
        this.propertiesTableBody.innerHTML = "";
        this.properties.forEach(function (property, index) {
            var row = document.createElement("tr");
            var propertyNameCell = document.createElement("td");
            var propertyDescriptionCell = document.createElement("td");
            var propertyisRequiredCell = document.createElement("td");
            var propertyisRequiredCheckBox = document.createElement("input");
            var propertyManageTranslationsCell = document.createElement("td");
            var manageTranslationsSpan = document.createElement("span");
            var propertyRemoveCell = document.createElement("td");
            var removePropertySpan = document.createElement("span");
            row.dataset.objectRef = index.toString();
            propertyNameCell.dataset.objectRefType = "name";
            propertyDescriptionCell.dataset.objectRefType = "description";
            propertyisRequiredCheckBox.dataset.objectRefType = "isRequired";
            propertyNameCell.innerText = property.Name;
            propertyDescriptionCell.innerText = property.Description;
            propertyisRequiredCheckBox.checked = property.isRequired;
            propertyisRequiredCell.className = "linkRow";
            propertyisRequiredCheckBox.type = "checkbox";
            propertyManageTranslationsCell.classList.add("linkRow");
            propertyRemoveCell.classList.add("linkRow");
            propertyisRequiredCheckBox.className = "w3-check";
            manageTranslationsSpan.className = "w3-text-theme link";
            manageTranslationsSpan.dataset.objectRef = index.toString();
            removePropertySpan.className = "w3-text-theme link";
            manageTranslationsSpan.innerText = _this.manageTranslationsText;
            removePropertySpan.innerText = _this.removePropertyText;
            propertyNameCell.addEventListener("dblclick", function (event) {
                return _this.tableCell_DblClick(propertyNameCell, event);
            });
            propertyDescriptionCell.addEventListener("dblclick", function (event) {
                return _this.tableCell_DblClick(propertyDescriptionCell, event);
            });
            propertyisRequiredCheckBox.addEventListener("click", function (event) {
                return _this.updateValues();
            });
            removePropertySpan.addEventListener("click", function (event) {
                return _this.removePropertySpan_Click(property, event);
            });
            manageTranslationsSpan.addEventListener("click", function (event) {
                return _this.manageTranslationsSpan_Click(property, event);
            });
            propertyisRequiredCell.appendChild(propertyisRequiredCheckBox);
            propertyManageTranslationsCell.appendChild(manageTranslationsSpan);
            propertyRemoveCell.appendChild(removePropertySpan);
            row.appendChild(propertyNameCell);
            row.appendChild(propertyDescriptionCell);
            row.appendChild(propertyisRequiredCell);
            row.appendChild(propertyManageTranslationsCell);
            row.appendChild(propertyRemoveCell);
            _this.propertiesTableBody.appendChild(row);
        });
        console.log(this.properties);
    };
    PropertiesTable.prototype.addProperty_Click = function (sender, event) {
        var name = this.newPropertyNameField.innerText;
        var description = this.newPropertyDescriptionField.innerText;
        var isRequired = this.newPropertyIsRequiredField.checked;
        var property = new Property(name, description);
        property.isRequired = isRequired;
        property.addTranslation(2, "", "");
        property.addTranslation(3, "", "");
        this.properties.push(property);
        this.updateTable();
        this.manageTranslationsSpan_Click(property, null);
        this.newPropertyNameField.innerText = "(Nosaukums)...";
        this.newPropertyDescriptionField.innerText = "(Apraksts)...";
        this.newPropertyIsRequiredField.checked = false;
    };
    PropertiesTable.prototype.submitButton_Click = function (sender, eventArgs) {
        event.preventDefault();
        var obj = {};
        var newCategoryForm = document.getElementById("newCategoryForm");
        var formData = new FormData(newCategoryForm);
        var request = new XMLHttpRequest();
        request.addEventListener("load", function (event) {
            var response = JSON.parse(request.response);
            console.log(response);
            if (response.result)
                window.location.href = "/Manage/Categories/Index";
            else if (response.errors) {
                var errors_1 = response.errors;
                Object.keys(errors_1).forEach(function (key) {
                    var keyErrors = errors_1[key];
                    if (keyErrors) {
                        var element_1 = document.getElementsByName(key);
                        if (element_1)
                            element_1[0].classList.add("validationError");
                        var element_ValidationError = document.getElementById(key + "_ValidationMessage");
                        if (element_ValidationError) {
                            var innHTML = "";
                            if (keyErrors.length > 1) {
                                innHTML = '<ul>';
                                for (var i = 0; i < keyErrors.length; i++) {
                                    innHTML += "<li>" + keyErrors[i] + "</li>";
                                }
                                innHTML += "</ul>";
                                element_ValidationError.innerHTML = innHTML;
                            }
                            else {
                                element_ValidationError.innerText = keyErrors[0];
                            }
                            element_ValidationError.classList.remove("w3-hide");
                        }
                        setTimeout(function () {
                            element_1[0].classList.remove("validationError");
                        }, 2000);
                    }
                });
            }
        });
        request.open("POST", "/Manage/Categories/New");
        request.setRequestHeader("Content-Type", "application/json");
        request.send(JSON.stringify(obj));
        return false;
    };
    return PropertiesTable;
}());
//# sourceMappingURL=PropertiesTable.js.map