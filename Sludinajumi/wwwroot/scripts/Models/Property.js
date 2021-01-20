var Property = (function () {
    function Property(neutralName, neutralDescription) {
        this.name = new TranslationEntry();
        this.name.neutralValue = neutralName;
        this.name.addTranslation(neutralLanguage, this.name.neutralValue);
        this.description = new TranslationEntry();
        this.description.neutralValue = neutralDescription;
        this.description.addTranslation(neutralLanguage, this.description.neutralValue);
        this.name.neutralDescription = "Īpašības nosaukums";
        this.description.neutralDescription = "Īpašības apraksts";
    }
    Property.fromObject = function (obj) {
        var prop = new Property("", "");
        prop.name = obj.name;
        prop.description = obj.description;
        prop.Name = obj.name.neutralValue;
        prop.Description = obj.description.neutralValue;
        prop.isRequired = obj.isRequired;
        return prop;
    };
    Object.defineProperty(Property.prototype, "TranslatedName", {
        get: function () {
            return this.name;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Property.prototype, "TranslatedDescription", {
        get: function () {
            return this.description;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Property.prototype, "Name", {
        get: function () {
            return this.name.neutralValue;
        },
        set: function (name) {
            this.name.neutralValue = name;
            this.name.translations[0].value = name;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Property.prototype, "Description", {
        get: function () {
            return this.description.neutralValue;
        },
        set: function (desc) {
            this.description.neutralValue = desc;
            this.description.translations[0].value = desc;
        },
        enumerable: true,
        configurable: true
    });
    Property.prototype.addTranslation = function (langId, name, description) {
        this.name.addTranslation(langId, name);
        this.description.addTranslation(langId, description);
    };
    return Property;
}());
export { Property };
//# sourceMappingURL=Property.js.map