const { motion, AnimatePresence } = window["framer-motion"];

const categories = {
    skincare: [
        { id: 1, name: "Hydrating Serum" },
        { id: 2, name: "Vitamin C Booster" },
        { id: 3, name: "Night Repair Cream" }
    ],
    cleansing: [
        { id: 4, name: "Foaming Cleanser" },
        { id: 5, name: "Micellar Water" },
        { id: 6, name: "Gentle Scrub" }
    ]
};

function ProductAnimatedGrid() {
    const [openCategory, setOpenCategory] = React.useState(null);

    const cardVariants = {
        closed: { height: 120 },
        open: { height: "auto" }
    };

    const itemVariants = {
        hidden: { opacity: 0, y: 15 },
        show: { opacity: 1, y: 0 }
    };

    return (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 p-6">
            {Object.keys(categories).map((category) => (
                <motion.div
                    key={category}
                    variants={cardVariants}
                    animate={openCategory === category ? "open" : "closed"}
                    className="overflow-hidden"
                >
                    <div
                        className="cursor-pointer rounded-2xl shadow-lg bg-white p-6 hover:shadow-xl transition"
                        onClick={() =>
                            setOpenCategory(openCategory === category ? null : category)
                        }
                    >
                        <div className="flex justify-between items-center">
                            <h2 className="text-xl font-semibold capitalize">
                                {category}
                            </h2>

                            <motion.span
                                animate={{ rotate: openCategory === category ? 180 : 0 }}
                                transition={{ duration: 0.3 }}
                            >
                                ▼
                            </motion.span>
                        </div>

                        <AnimatePresence>
                            {openCategory === category && (
                                <motion.div
                                    initial="hidden"
                                    animate="show"
                                    exit="hidden"
                                    className="space-y-2 pt-3"
                                >
                                    {categories[category].map((product, index) => (
                                        <motion.div
                                            key={product.id}
                                            variants={itemVariants}
                                            transition={{ delay: index * 0.1 }}
                                            className="p-3 rounded-xl bg-gray-100 hover:bg-gray-200 transition"
                                        >
                                            {product.name}
                                        </motion.div>
                                    ))}
                                </motion.div>
                            )}
                        </AnimatePresence>
                    </div>
                </motion.div>
            ))}
        </div>
    );
}

ReactDOM.createRoot(document.getElementById("product-animated-grid")).render(
    <ProductAnimatedGrid />
);
